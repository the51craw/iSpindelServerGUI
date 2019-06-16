using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;

namespace iSpindelServerGUI
{
    /// <summary>
    /// iSpindel configuration: TCP IP address and port.
    /// Port and IP address configurable.
    /// Note: iSpindel and computer must use the same network.
    ///     it is not possible to connect iSpindel to a wi-fi
    ///     network if the computer is connected via cable.
    /// </summary>
    public partial class MainWindow : Window
    {
        public IPEndPoint localEndPoint { get; set; }
        public Socket clientSocket { get; set; }
        public Socket inputSocket { get; set; }
        public byte[] inputBuffer { get; set; }
        public byte[] outputBuffer { get; set; }

        public Logger logger;
        private ByteBuffPrinter byteBuffPrinter;
        private String filePath;

        private DispatcherTimer timer;
        private bool dataIsAvailable;
        private bool stop;
        private int buffpointer;
        private SocketAsyncEventArgs socketAsyncEventArgs;

        private string key = "HKEY_LOCAL_MACHINE\\SOFTWARE\\iSpindelServerGUI";
        private string valuePath = "FilePath";
        private string valueName = "FileName";

        private Boolean initDone = false;

        public BackgroundWorker listener { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            AdminRelauncher();
            Init();
        }

        private void AdminRelauncher()
        {
            if (!IsRunAsAdmin())
            {
                ProcessStartInfo proc = new ProcessStartInfo();
                proc.UseShellExecute = true;
                proc.WorkingDirectory = Environment.CurrentDirectory;
                proc.FileName = Assembly.GetEntryAssembly().CodeBase;

                proc.Verb = "runas";

                try
                {
                    Process.Start(proc);
                    Application.Current.Shutdown();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("This program must be run as an administrator! \n\n" + ex.ToString());
                }
            }
        }

        private bool IsRunAsAdmin()
        {
            try
            {
                WindowsIdentity id = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(id);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (Exception)
            {
                return false;
            }
        }   
        private void Init()
        {
            btnCloseConnection.IsEnabled = false;
            stop = false;
            txtIndata.Text = "";
            inputBuffer = new byte[1];
            outputBuffer = new byte[1024];
            socketAsyncEventArgs = new SocketAsyncEventArgs();
            socketAsyncEventArgs.SetBuffer(inputBuffer, 0, 1);
            socketAsyncEventArgs.Completed += SocketAsyncEventArgs_Completed;
            tbFileName.Text = ReadNameFromSettings();
            logger = new Logger();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            initDone = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (dataIsAvailable)
            {
                dataIsAvailable = false;
                txtIndata.Text = "";
                ISpindelData data = logger.GetValues();
                String values = "";
                values += "Angle: " + data.AngleValues[data.AngleValues.Count() - 1] + ", ";
                values += "Temperature: " + data.TemperatureValues[data.TemperatureValues.Count() - 1] + ", ";
                values += "Battery: " + data.BatteryValues[data.BatteryValues.Count() - 1] + ", ";
                values += "Gravity: " + data.GravityValues[data.GravityValues.Count() - 1];

                txtIndata.Text = values;
                btnDrawDiagram_Click(null, null);
            }
        }

        private void btnOpenConnection_Click(object sender, RoutedEventArgs e)
        {
            byteBuffPrinter = new ByteBuffPrinter();

            txtIndata.Text = "";
            buffpointer = 0;
            btnCloseConnection.IsEnabled = true;
            btnOpenConnection.IsEnabled = false;
            tbFileName.IsEnabled = false;
            btnBrowse.IsEnabled = false;
            localEndPoint = new IPEndPoint(IPAddress.Any, 35418);
            inputSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            inputSocket.Bind(localEndPoint);
            listener = new BackgroundWorker();
            listener.DoWork += Listener_DoWork;
            logger.OpenExcel();
            listener.RunWorkerAsync();
        }

        private async void Listener_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                inputSocket.Listen(1000);
                clientSocket = await inputSocket.AcceptAsync();
                while (!dataIsAvailable)
                {
                    clientSocket.Receive(inputBuffer);
                    if (inputBuffer[0] == (byte)'{')
                    {
                        buffpointer = 0;
                    }
                    outputBuffer[buffpointer++] = inputBuffer[0];
                    if (outputBuffer[buffpointer - 1] == (byte)'}')
                    {
                        dataIsAvailable = true;
                    }
                }
                logger.Add(byteBuffPrinter.AsciiToString(outputBuffer, '}'));
            }
        }

        private void SocketAsyncEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            dataIsAvailable = true;
        }

        private void btnCloseConnection_Click(object sender, RoutedEventArgs e)
        {
            logger.CloseExcel();
            btnCloseConnection.IsEnabled = false;
            btnOpenConnection.IsEnabled = true;
            tbFileName.IsEnabled = true;
            btnBrowse.IsEnabled = true;
            clientSocket.Close();
            clientSocket.Dispose();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            String filePath = ReadPathFromSettings();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            if (!String.IsNullOrEmpty(filePath))
            {
                openFileDialog.InitialDirectory = filePath;
            }
            if ((Boolean)openFileDialog.ShowDialog(this))
            {
                logger.FileName = openFileDialog.FileName;
                filePath = openFileDialog.FileName.Remove(openFileDialog.FileName.LastIndexOf('\\'));
                WriteSettings(filePath);
                tbFileName.Text = openFileDialog.FileName;
            }
        }

        private String ReadPathFromSettings()
        {
            return (String)Registry.GetValue(key, valuePath, "");
        }

        private String ReadNameFromSettings()
        {
            return (String)Registry.GetValue(key, valueName, "");
        }

        private void WriteSettings(String FilePath)
        {
            Registry.SetValue(key, valuePath, FilePath);
        }

        private void btnDrawDiagram_Click(object sender, RoutedEventArgs e)
        {
            double height = canvasDiagram.ActualHeight;
            double width = canvasDiagram.ActualWidth;
            Rectangle rectangle = new Rectangle();
            rectangle.Fill = new SolidColorBrush(Colors.White);
            rectangle.Stretch = Stretch.Fill;
            rectangle.Width = width;
            rectangle.Height = height;
            canvasDiagram.Children.Add(rectangle);
            
            ISpindelData data = logger.GetValues();
            if (data.AngleValues.Count > 1)
            {
                double steplength = width / (data.AngleValues.Count() - 1);
                for (int i = 1; i < data.AngleValues.Count(); i++)
                {
                    Line line = new Line();
                    line.X1 = (i - 1) * steplength;
                    line.X2 = i * steplength;
                    line.Y1 = height - (data.AngleValues[i - 1] * height / 90);
                    line.Y2 = height - (data.AngleValues[i] * height / 90);
                    line.StrokeThickness = 1;
                    line.Stroke = new SolidColorBrush(Colors.Black);
                    canvasDiagram.Children.Add(line);
                }
            }
        }

        private void TbFileName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (initDone)
            {
                logger.FileName = tbFileName.Text;
                filePath = tbFileName.Text.Remove(tbFileName.Text.LastIndexOf('\\'));
                WriteSettings(filePath);
            }
        }
    }
}
