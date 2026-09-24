using System;

namespace GPC.Utilities.Maths
{
    public static class Interpolation
    {
        /// <remarks>If <paramref name="x1"/> is equal to <paramref name="x2"/> returns <paramref name="y1"/></remarks>
        public static double GetLinearInterpolation(double x1, double x2, double y1, double y2, double x)
        {
            if (x2 == x1)
                return y1;

            return y1 + (y2 - y1) / (x2 - x1) * (x - x1);
        }

        /// <param name="xi">The abscissae, in ascending order</param>
        /// <param name="yi">The ordinates</param>
        /// <param name="x">The abscissa where to interpolate</param>
        /// <returns>The interpolated value. Outside the range of <paramref name="xi"/> the first or last value of <paramref name="yi"/> is returned</returns>
        /// <exception cref="ArgumentException">If the arrays are empty or have different lengths</exception>
        public static double GetLinearInterpolation(double[] xi, double[] yi, double x)
        {
            if (xi is null)
                throw new ArgumentNullException(nameof(xi));
            if (yi is null)
                throw new ArgumentNullException(nameof(yi));
            if (xi.Length != yi.Length)
                throw new ArgumentException("Lenght of vectors are different");
            if (xi.Length == 0)
                throw new ArgumentException("Vectors cannot be empty");

            if (x <= xi[0])
            {
                return yi[0];
            }
            else if (x >= xi[xi.Length - 1])
            {
                return yi[yi.Length - 1];
            }
            else
            {
                for (int i = 0; i < xi.Length - 1; i++)
                {
                    if (x >= xi[i] && x <= xi[i + 1])
                    {
                        return GetLinearInterpolation(xi[i], xi[i + 1], yi[i], yi[i + 1], x);
                    }
                }
                throw new ArgumentException("Abscissae must be in ascending order", nameof(xi));
            }
        }

        public static double GetQuadraticInterpolation(double x1, double x2, double x3, double y1, double y2, double y3, double x)
        {
            return y1 * ((x - x2) * (x - x3) / ((x1 - x2) * (x1 - x3))) +
                y2 * ((x - x1) * (x - x3) / ((x2 - x1) * (x2 - x3))) +
                y3 * ((x - x1) * (x - x2) / ((x3 - x1) * (x3 - x2)));
        }
    }
}
