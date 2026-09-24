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
        /// <remarks>Process and thread priority are raised during the measure and restored at the end</remarks>
        public static double FunctionExecutionTime(int iterations, Action func, bool printResultsToConsole = false, string description = "")
        {
            return FunctionExecutionTime(iterations, () => { func(); return null; }, out _, printResultsToConsole, description);
        }

        /// <returns>The total time elapsed divided by the <paramref name="iterations"/> number</returns>
        /// <remarks>Process and thread priority are raised during the measure and restored at the end</remarks>
        public static double FunctionExecutionTime(int iterations, Func<object> func, out object returnObject, bool printResultsToConsole = false, string description = "")
        {
            if (iterations < 1)
                throw new ArgumentOutOfRangeException(nameof(iterations), iterations, "Iterations must be greater than zero");

            Process process = Process.GetCurrentProcess();
            ProcessPriorityClass oldProcessPriority = process.PriorityClass;
            ThreadPriority oldThreadPriority = Thread.CurrentThread.Priority;

            try
            {
                process.PriorityClass = ProcessPriorityClass.High;
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                returnObject = null;

                func.Invoke(); // warm up (JIT)

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
            finally
            {
                process.PriorityClass = oldProcessPriority;
                Thread.CurrentThread.Priority = oldThreadPriority;
            }
        }
    }
}
