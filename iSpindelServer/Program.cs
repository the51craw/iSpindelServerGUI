using System;
using System.ServiceModel;

namespace iSpindelServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //IiSpindelServiceLibraryClient client = new IiSpindelServiceLibraryClient();
            //Console.WriteLine(client.GetData(7));
            // C:\Windows\assembly\GAC_MSIL\System.ServiceModel\3.0.0.0__b77a5c561934e089\System.ServiceModel.dll
            using (ServiceHost host = new ServiceHost(typeof(iSpindelServiceLibrary.iSpindelServiceLibrary)))
            {
                host.Open();

                Console.WriteLine("Service up and running at:");
                foreach (var ea in host.Description.Endpoints)
                {
                    Console.WriteLine(ea.Address);
                }

                Console.ReadLine();
                host.Close();
            }
            //client.Close();
            Console.ReadKey();
        }
    }
}
