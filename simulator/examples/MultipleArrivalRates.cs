using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace simulator.examples
{
    class MultipleArrivalRates
    {
        public void start()
        {
            Task t = new EventGenerator(1d / 5).Start(() => {
                Console.WriteLine("rapid events"); });
            Task t1 = new EventGenerator(1d / 20).Start(() => {
                Console.WriteLine("once every 20 seconds (on average)"); });
            Task t2 = new EventGenerator(1d / 360).Start(() => {
                Console.WriteLine("once every HOUR"); });
        }
    }

}
