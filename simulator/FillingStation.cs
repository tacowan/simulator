using MathNet.Numerics.Distributions;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;

namespace simulator
{
    public class FillingStationEvent
    {
        public string EventType { get; set; }
        public int FuelType { get; set; }
        public float Galons { get; set; }
        public string Pump { get; set; }

        public FillingStationEvent(String t)
        {
            EventType = t;            
        }
    }
    class FillingStation
    {
        private DeviceClient s_deviceClient;
        private BlockingCollection<string> pumps;
        private BlockingCollection<string> waitingCars;
        private int total = 0;

        public FillingStation(DeviceClient d)
        {
            s_deviceClient = d;
            waitingCars = new BlockingCollection<string>();
            pumps = new BlockingCollection<string>();
            for (int i=0; i<12; i++)
            {
                pumps.Add("pump #" + i);
            }
            
        }

        public void SendDeviceToCloudMessagesAsync()
        {

            Task t = new EventGenerator(10d / 60).Start(() =>
            {
                Console.WriteLine("new car arrival, {0} in queue", waitingCars.Count);
                if (waitingCars.Count < 4)
                    waitingCars.Add("car #" + total++);
                else
                    s_deviceClient.SendEventAsync(getMessage("Lot Full", "", 0));
            });

            while(true)
            {
                string car = waitingCars.Take(); //block until car arrives
                string pump = pumps.Take(); //block until a pump is available
                var t2 = Task.Factory.StartNew( async () =>
                {
                    // average time at pump, 1 minute, 15 second standard deviation
                    // agressive so that we can see events fire, adjust as needed
                    Normal normalDist = new Normal(60, 15);
                    double randTime = normalDist.Sample();
                                                           
                    Console.WriteLine(pump + " start");
                    await s_deviceClient.SendEventAsync(getMessage("Start", pump, 0));
                    await Task.Delay((int)randTime * 1000);

                    Console.WriteLine("pump end after {0} seconds", (int)randTime);
                    await s_deviceClient.SendEventAsync(getMessage("End", pump, 20));
                    pumps.Add(pump);
                });
            }
            
           
        }

        private Message getMessage(String eventType, String pump, int gallons)
        {
            var telemetry = new FillingStationEvent(eventType);
            telemetry.Galons = gallons;
            telemetry.Pump = pump;
            var messageString = JsonConvert.SerializeObject(telemetry);
            return new Message(Encoding.ASCII.GetBytes(messageString));
        }
    } 
}
