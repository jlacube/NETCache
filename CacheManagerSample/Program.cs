using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NETCache.CacheManager;
using NETCache.CacheManager.Interfaces;
using NETCache.CacheTools;

namespace CacheManagerSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();

            CustomTimer timer = new CustomTimer();

            Console.WriteLine("ICacheManager<int, int>");
            for (int j = 0; j < 10; ++j)
            {
                ICacheManager<int, int> cacheManager = new CacheManager<int, int>();

                timer.Start();

                // No concurrency insertions
                for (int i = 0; i < 1000000; ++i)
                {
                    cacheManager.AddEntry(random.Next(int.MaxValue), i);
                }

                timer.StopAndReport();

                GC.Collect(2);
                GC.WaitForFullGCComplete();
            }
            Console.WriteLine("");

            Console.WriteLine("ICacheManager<int, string>");
            for (int j = 0; j < 10; ++j)
            {
                ICacheManager<int, string> cacheManager = new CacheManager<int, string>();

                timer.Start();

                // No concurrency insertions
                for (int i = 0; i < 1000000; ++i)
                {
                    cacheManager.AddEntry(random.Next(int.MaxValue), "dummy string " + i);
                }

                timer.StopAndReport();

                GC.Collect(2);
                GC.WaitForFullGCComplete();
            }
            Console.WriteLine("");

            Console.WriteLine("ICacheManager<int, string, int>");
            for (int j = 0; j < 10; ++j)
            {
                ICacheManager<int, string, int> cacheManager = new CacheManager<int, string, int>();

                timer.Start();

                // No concurrency insertions
                for (int i = 0; i < 1000000; ++i)
                {
                    cacheManager.AddEntry(random.Next(int.MaxValue), "dummy string " + i);
                }

                timer.StopAndReport();

                GC.Collect(2);
                GC.WaitForFullGCComplete();
            }
            Console.WriteLine("");

            Console.ReadLine();
        }
    }
}
