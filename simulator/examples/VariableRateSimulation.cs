using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

// simulate events that arrive at steadily decreasing or increasing
// rate.  (people leaving or comming to work )
namespace simulator.examples
{
    /// Simulate radomly timed events that arrive at steadily decreasing rate.
    /// 
    /// ex. people leaving work will leave at a high rate at 5pm, and the rate decreases
    /// 
    class VariableRateSimulation
    {
        public VariableRateSimulation()
        {

        }

        public void start()
        {
            EventGenerator poisson = new EventGenerator(1);
            Task t = Task.Factory.StartNew(async () => {
                int timeintervals = 1;
                double arrivalRate = poisson.ArrivalRate;
                Console.WriteLine(arrivalRate);
                while (true)
                {
                    await Task.Delay(1000 * 10);
                    double newrate = arrivalRate * Math.Exp(-timeintervals * 1d/5);
                    poisson.ArrivalRate = newrate;
                    Console.WriteLine(newrate);
                    timeintervals++;                  
                }
            });


            int e = 0;
            Task t2 = poisson.Start(() =>
            {               
                Console.WriteLine("event " + e++);                
            });

        }
    }
}
