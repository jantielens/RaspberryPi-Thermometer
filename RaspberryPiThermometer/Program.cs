using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryPiThermometer
{
    class Program
    {
        public static MobileServiceClient MobileService = new MobileServiceClient(
            "https://cloudthermometer.azure-mobile.net/",
            "CETpZxkiFLxhQCnZvXKkqgllxDpNew44"
        );


        static void Main(string[] args)
        {
            System.Timers.Timer t = new System.Timers.Timer(2000);
            t.Elapsed += (s, e) =>
            {
                Console.Write("Sending ... ");
                double temp = GetTemperature(0);
                Console.Write(temp.ToString());

                try
                {
                    MobileService.GetTable<SensorValue>().InsertAsync(
                              new SensorValue()
                                  {
                                      SensorName = "Raspberry Pi",
                                      Timestamp = DateTime.Now,
                                      Value = temp
                                  }
                                );
                    Console.WriteLine(" ... done!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                t.Enabled = true;
            };

            t.AutoReset = false;
            t.Start();
            Console.ReadKey();
            t.Stop();
        }

        static double GetTemperature(int sensorIndex)
        {
            DirectoryInfo devicesDir = new DirectoryInfo("/sys/bus/w1/devices");
            var deviceDir = devicesDir.GetDirectories("28*")[sensorIndex];
            using (TextReader reader = 
                new System.IO.StreamReader(deviceDir.FullName + "/w1_slave"))
            {
                string w1slavetext = reader.ReadToEnd();

                string temptext =
                    w1slavetext.Split(new string[] { "t=" }, 
                    StringSplitOptions.RemoveEmptyEntries)[1];

                double temp = double.Parse(temptext) / 1000;
                return temp;
            }
        }
    }
}
