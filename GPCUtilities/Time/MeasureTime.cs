using System;
using System.Diagnostics;
using System.Threading;

namespace GPC.Utilities.Time
{
    public static class MeasureTime
    {
        /// <param name="iterations"></param>
        /// <param name="func"></param>
        /// <param name="printResultsToConsole"></param>
        /// <param name="description"></param>
        /// <returns>The total time elapsed divided by the <paramref name="iterations"/> number</returns>
        public static double FunctionExecutionTime(int iterations, Action func, bool printResultsToConsole = false, string description = "")
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            func();
            var watch = new Stopwatch();

            // clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            watch.Start();
            for (int i = 0; i < iterations; i++)
            {
                func();
            }
            watch.Stop();

            if (printResultsToConsole)
            {
                Console.WriteLine(description);
                Console.WriteLine($" Iterations {iterations}");
                Console.WriteLine($" Total Time Elapsed {watch.Elapsed.TotalMilliseconds} ms");
                Console.WriteLine($" Time Elapsed/Iterations {watch.Elapsed.TotalMilliseconds / iterations} ms");
            }
            return watch.Elapsed.TotalMilliseconds / iterations;
        }

        /// <returns>The total time elapsed divided by the <paramref name="iterations"/> number</returns>
        public static double FunctionExecutionTime(int iterations, Func<object> func, out object returnObject, bool printResultsToConsole = false, string description = "")
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            returnObject = null;

            func.Invoke();

            var watch = new Stopwatch();

            // clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            watch.Start();
            for (int i = 0; i < iterations; i++)
            {
                returnObject = func.Invoke();
            }
            watch.Stop();

            if (printResultsToConsole)
            {
                Console.WriteLine(description);
                Console.WriteLine($" Iterations {iterations}");
                Console.WriteLine($" Total Time Elapsed {watch.Elapsed.TotalMilliseconds} ms");
                Console.WriteLine($" Time Elapsed/Iterations {watch.Elapsed.TotalMilliseconds / iterations} ms");
            }

            return watch.Elapsed.TotalMilliseconds / iterations;
        }
    }
}