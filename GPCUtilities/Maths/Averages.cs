using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPC.Utilities.Maths
{
    public static class Averages
    {
        /// <returns>The Arithmetic Mean value</returns>
        // http://www.heikohoffmann.de/htmlthesis/node134.html
        public static double ArithmeticMean(IEnumerable<double> values)
        {
            double average = 0;

            int i = 0;
            foreach(var value in values)
            {
                average += 1.0 / (i++ + 1.0) * (value - average);
            }

            return average;
        }

        /// <returns>The Arithmetic Mean value</returns>
        // http://www.heikohoffmann.de/htmlthesis/node134.html
        public static double ArithmeticMean(IEnumerable<int> values)
        {
            double average = 0;

            int i = 0;
            foreach (var value in values)
            {
                average += 1.0 / (i++ + 1.0) * (value - average);
            }

            return average;
        }
    }
}
