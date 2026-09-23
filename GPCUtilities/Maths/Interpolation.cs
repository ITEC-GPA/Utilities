using System;

namespace GPC.Utilities.Maths
{
    public static class Interpolation
    {
        public static double GetLinearInterpolation(double x1, double x2, double y1, double y2, double x)
        {
            return y1 + (y2 - y1) / (x2 - x1) * (x - x1);
        }

        public static double GetLinearInterpolation(double[] xi, double[] yi, double x)
        {
            if (xi.Length != yi.Length)
            {
                throw new ArgumentException("Lenght of vectors are different");
            }
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
                throw new NotSupportedException();
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