using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace iSpindelServerGUI
{
    public class Logger
    {
        public ISpindelData ISpindelData;
        public String FileName { get {return fileName; }  set { fileName = value;  WriteSettings(value); } }
        private String fileName;

        private Microsoft.Office.Interop.Excel.Application oXL;
        private Microsoft.Office.Interop.Excel._Workbook oWB;
        private Microsoft.Office.Interop.Excel._Worksheet oSheet;
        private Int32 row = 2;

        private string key = "HKEY_LOCAL_MACHINE\\SOFTWARE\\iSpindelServerGUI";
        private string valueName = "FileName";

        public Logger()
        {
            ISpindelData = new ISpindelData();
        }

        public void Clear()
        {
            ISpindelData = new ISpindelData();
        }

        public void OpenExcel()
        {
            fileName = ReadSettings();
            if (!String.IsNullOrEmpty(fileName))
            {
                try
                {
                    if (oXL == null)
                    {
                        oXL = new Microsoft.Office.Interop.Excel.Application();
                        oXL.Visible = true;
                    }
                    Int32 row = 2;

                    if (File.Exists(fileName))
                    {
                        oWB = oXL.Workbooks.Open(fileName);
                        oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;
                    }
                    else
                    {
                        oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add());
                        oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;
                        oSheet.Cells[1, 1] = "Date and time";
                        oSheet.Cells[1, 2] = "Angle";
                        oSheet.Cells[1, 3] = "Temperature";
                        oSheet.Cells[1, 4] = "Battery";
                        oSheet.Cells[1, 5] = "Gravity";
                    }
                }
                catch (Exception e)
                {
                    if (oWB != null)
                    {
                        oWB.Close();
                    }
                }
            }
        }

        public void CloseExcel()
        {
            try
            {
                oWB.SaveAs(fileName);
                oWB.Close();
                oXL.Visible = false;
                oXL.UserControl = false;
                if (oWB != null)
                {
                    oWB.Close();
                }
            }
            catch (Exception e)
            {

            }
        }

        public void Add(string jSonData)
        {
            JObject jObject = JObject.Parse(jSonData);
            double angle = (double)jObject.GetValue("angle");
            ISpindelData.AngleValues.Add(angle);
            double temperature = (double)jObject.GetValue("temperature");
            ISpindelData.TemperatureValues.Add(temperature - 2.3); // After comparison with room temperature. 
            double battery = (double)jObject.GetValue("battery");
            ISpindelData.BatteryValues.Add(battery * 10); // To make it readable in diagram together with the other lines, 40 = 4 Volt.
            double gravity = (double)jObject.GetValue("gravity");
            ISpindelData.GravityValues.Add(gravity);
            SaveToExcel(jSonData);
        }

        private void SaveToExcel(String jSonData)
        {
            fileName = ReadSettings();
            if (!String.IsNullOrEmpty(fileName))
            {
                try
                {
                    if (oXL != null)
                    {
                        String temp = oSheet.Cells[row, 1].Text;
                        while (temp != "")
                        {
                            temp = oSheet.Cells[++row, 1].Text;
                        }
                        JObject jObject = JObject.Parse(jSonData);
                        oSheet.Cells[row, 1] = DateTime.Now.ToLocalTime();
                        double angle = (double)jObject.GetValue("angle");
                        oSheet.Cells[row, 2] = angle;
                        double temperature = (double)jObject.GetValue("temperature");
                        oSheet.Cells[row, 3] = temperature;
                        double battery = (double)jObject.GetValue("battery");
                        oSheet.Cells[row, 4] = battery;
                        double gravity = (double)jObject.GetValue("gravity");
                        oSheet.Cells[row, 5] = gravity;
                    }
                }
                catch (Exception e)
                {
                    if (oWB != null)
                    {
                        oWB.Close();
                    }
                }
            }
        }

        private String ReadSettings()
        {
            return (String)Registry.GetValue(key, valueName, "");
        }

        private void WriteSettings(String FileName)
        {
            Registry.SetValue(key, valueName, FileName);
        }

        public ISpindelData GetValues()
        {
            return ISpindelData;
        }
    }

    public class ISpindelData
    {
        public List<double> AngleValues { get; set; }
        public List<double> TemperatureValues { get; set; }
        public List<double> BatteryValues { get; set; }
        public List<double> GravityValues { get; set; }

        public ISpindelData()
        {
            AngleValues = new List<double>();
            TemperatureValues = new List<double>();
            BatteryValues = new List<double>();
            GravityValues = new List<double>();
        }
    }
}
