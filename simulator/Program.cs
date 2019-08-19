using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using simulator.examples;

namespace simulator
{
    class Program
    {
        private static DeviceClient s_deviceClient;
        //private readonly static string s_connectionString = 
        static void Main(string[] args)
        {
            Console.WriteLine("Simulation started");
            //var s_connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            //s_deviceClient = DeviceClient.CreateFromConnectionString(s_connectionString, TransportType.Mqtt);
            //FillingStation fillingStation = new FillingStation(s_deviceClient);
            //fillingStation.SendDeviceToCloudMessagesAsync();

            new MultipleArrivalRates().start();
            Console.ReadLine();
        }     
    }
}
