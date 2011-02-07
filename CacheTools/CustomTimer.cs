using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NETCache.CacheTools
{
    public class CustomTimer
    {
        private DateTime startTime = DateTime.MinValue;
        private DateTime endTime = DateTime.MinValue;

        public void Start()
        {
            startTime = DateTime.UtcNow;
        }

        public void Stop()
        {
            endTime = DateTime.UtcNow;
        }

        public void Report()
        {
            TimeSpan delta = endTime - startTime;
            Console.WriteLine("{0}h{1}m{2}s{3}ms", delta.Hours, delta.Minutes, delta.Seconds, delta.Milliseconds);
        }

        public void StopAndReport()
        {
            Stop();
            Report();
        }
    }
}
