using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace simulator
{
    
    class EventGenerator
    {

        public double ArrivalRate { get; set; }

        public EventGenerator(double eventsPerSecond)
        {
            ArrivalRate = eventsPerSecond;
        }

        public async Task<int> Start(Action a)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            while (true)
            {
                double d = Math.Log(random.NextDouble()) / -ArrivalRate;
                await Task.Delay((int)(d * 1000));
                a.Invoke();
            }
        }
    }
}
