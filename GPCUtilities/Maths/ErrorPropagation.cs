using System;

namespace GPC.Utilities.Maths
{
    public static class ErrorPropagation
    {
        /*
        https://it.wikipedia.org/wiki/Propagazione_degli_errori#:~:text=In%20fisica%2C%20per%20propagazione%20degli,ad%20una%20funzione%20di%20esso
        */

        /// <summary>
        /// Calculate the square of the tolerance of the result of the sum of two terms
        /// </summary>
        /// <param name="ta">Tolerance or uncertainty of the first term of the sum</param>
        /// <param name="tb">Tolerance or uncertainty of the second term of the sum</param>
        /// <returns>The square of the tolerance</returns>
        /// <exception cref="ArgumentOutOfRangeException">If a tolerance is negative</exception>
        public static double SumSquareTolerance(double ta, double tb)
        {
            CheckTolerance(ta, nameof(ta));
            CheckTolerance(tb, nameof(tb));

            return ta * ta + tb * tb;
        }

        /// <summary>
        /// Calculate the tolerance of the result of the sum of two terms
        /// </summary>
        /// <param name="ta">Tolerance or uncertainty of the first term of the sum</param>
        /// <param name="tb">Tolerance or uncertainty of the second term of the sum</param>
        /// <returns>The value of tolerance</returns>
        /// <exception cref="ArgumentOutOfRangeException">If a tolerance is negative</exception>
        public static double SumTolerance(double ta, double tb)
        {
            return Math.Sqrt(SumSquareTolerance(ta, tb));
        }

        /// <summary>
        /// Calculate the square of the tolerance of the result of the product of two terms
        /// </summary>
        /// <param name="a">First term of the product</param>
        /// <param name="b">Second term of the product</param>
        /// <param name="ta">Tolerance or uncertainty of the first term of the product</param>
        /// <param name="tb">Tolerance or uncertainty of the second term of the product</param>
        /// <param name="minTolerance">Minimum value of the returned square tolerance. If lower or equal to zero, <paramref name="ta"/> * <paramref name="tb"/> is used</param>
        /// <returns>The square of the tolerance</returns>
        /// <exception cref="ArgumentOutOfRangeException">If a tolerance is negative</exception>
        public static double ProductSquareTolerance(double a, double b, double ta, double tb, double minTolerance = 0)
        {
            CheckTolerance(ta, nameof(ta));
            CheckTolerance(tb, nameof(tb));

            if (minTolerance <= 0)
                minTolerance = ta * tb;

            double tol = tb * tb * a * a + ta * ta * b * b;
            return tol < minTolerance ? minTolerance : tol;
        }

        /// <summary>
        /// Calculate the tolerance of the result of the product of two terms
        /// </summary>
        /// <param name="a">First term of the product</param>
        /// <param name="b">Second term of the product</param>
        /// <param name="ta">Tolerance or uncertainty of the first term of the product</param>
        /// <param name="tb">Tolerance or uncertainty of the second term of the product</param>
        /// <param name="minTolerance">Minimum value of the square tolerance. If lower or equal to zero, <paramref name="ta"/> * <paramref name="tb"/> is used</param>
        /// <returns>The value of tolerance</returns>
        /// <exception cref="ArgumentOutOfRangeException">If a tolerance is negative</exception>
        public static double ProductTolerance(double a, double b, double ta, double tb, double minTolerance = 0)
        {
            return Math.Sqrt(ProductSquareTolerance(a, b, ta, tb, minTolerance));
        }

        /// <summary>
        /// Calculate the square of the tolerance of the result of the product of three terms
        /// </summary>
        /// <param name="a">First term of the product</param>
        /// <param name="b">Second term of the product</param>
        /// <param name="c">Third term of the product</param>
        /// <param name="ta">Tolerance or uncertainty of the first term of the product</param>
        /// <param name="tb">Tolerance or uncertainty of the second term of the product</param>
        /// <param name="tc">Tolerance or uncertainty of the third term of the product</param>
        /// <param name="minTolerance">Minimum value of the returned square tolerance. If lower or equal to zero, <paramref name="ta"/> * <paramref name="tb"/> * <paramref name="tc"/> is used</param>
        /// <returns>The square of the tolerance</returns>
        /// <exception cref="ArgumentOutOfRangeException">If a tolerance is negative</exception>
        public static double ProductSquareTolerance(double a, double b, double c, double ta, double tb, double tc, double minTolerance = 0)
        {
            CheckTolerance(ta, nameof(ta));
            CheckTolerance(tb, nameof(tb));
            CheckTolerance(tc, nameof(tc));

            if (minTolerance <= 0)
                minTolerance = ta * tb * tc;

            double bc = b * c;
            double ac = a * c;
            double ab = a * b;
            double tol = ta * ta * bc * bc + tb * tb * ac * ac + tc * tc * ab * ab;
            return tol < minTolerance ? minTolerance : tol;
        }

        /// <summary>
        /// Calculate the tolerance of the result of the product of three terms
        /// </summary>
        /// <param name="a">First term of the product</param>
        /// <param name="b">Second term of the product</param>
        /// <param name="c">Third term of the product</param>
        /// <param name="ta">Tolerance or uncertainty of the first term of the product</param>
        /// <param name="tb">Tolerance or uncertainty of the second term of the product</param>
        /// <param name="tc">Tolerance or uncertainty of the third term of the product</param>
        /// <param name="minTolerance">Minimum value of the square tolerance. If lower or equal to zero, <paramref name="ta"/> * <paramref name="tb"/> * <paramref name="tc"/> is used</param>
        /// <returns>The value of tolerance</returns>
        /// <exception cref="ArgumentOutOfRangeException">If a tolerance is negative</exception>
        public static double ProductTolerance(double a, double b, double c, double ta, double tb, double tc, double minTolerance = 0)
        {
            return Math.Sqrt(ProductSquareTolerance(a, b, c, ta, tb, tc, minTolerance));
        }

        /// <summary>
        /// Calculate the tolerance of the result of the product of two terms in case there aren't two explicit terms of product or in case of squaring (exponentiation of power 2)
        /// </summary>
        /// <param name="t">Tolerance</param>
        /// <returns>The square of the tolerance</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the tolerance is negative</exception>
        public static double DefaultProductSquareTolerance(double t)
        {
            CheckTolerance(t, nameof(t));

            return t * t * 1.4142;
        }

        /// <summary>
        /// Calculate the tolerance of the result of the product of two terms in case there aren't two explicit terms of product or in case of squaring (exponentiation of power 2)
        /// </summary>
        /// <param name="t">Tolerance</param>
        /// <returns>The value of tolerance</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the tolerance is negative</exception>
        public static double DefaultProductTolerance(double t)
        {
            return Math.Sqrt(DefaultProductSquareTolerance(t));
        }

        /// <summary>
        /// Calculate the square of the tolerance of the result of the division of two terms
        /// </summary>
        /// <param name="a">First term of the division</param>
        /// <param name="b">Second term of the division</param>
        /// <param name="ta">Tolerance or uncertainty of the first term of the division</param>
        /// <param name="tb">Tolerance or uncertainty of the second term of the division</param>
        /// <param name="minTolerance">Minimum value of the returned square tolerance. If lower or equal to zero, <paramref name="ta"/> * <paramref name="tb"/> is used</param>
        /// <returns>The square of the tolerance</returns>
        /// <exception cref="ArgumentOutOfRangeException">If a tolerance is negative</exception>
        /// <exception cref="ArgumentException">If <paramref name="b"/> is zero</exception>
        public static double DivisionSquareTolerance(double a, double b, double ta, double tb, double minTolerance = 0)
        {
            CheckTolerance(ta, nameof(ta));
            CheckTolerance(tb, nameof(tb));

            if (b == 0)
                throw new ArgumentException("The divisor cannot be zero", nameof(b));

            if (minTolerance <= 0)
                minTolerance = ta * tb;

            double b2 = b * b;
            double tol = ta * ta / b2 + a * a * tb * tb / (b2 * b2);
            return tol < minTolerance ? minTolerance : tol;
        }

        /// <summary>
        /// Calculate the tolerance of the result of the division of two terms
        /// </summary>
        /// <param name="a">First term of the division</param>
        /// <param name="b">Second term of the division</param>
        /// <param name="ta">Tolerance or uncertainty of the first term of the division</param>
        /// <param name="tb">Tolerance or uncertainty of the second term of the division</param>
        /// <param name="minTolerance">Minimum value of the square tolerance. If lower or equal to zero, <paramref name="ta"/> * <paramref name="tb"/> is used</param>
        /// <returns>The value of tolerance</returns>
        /// <exception cref="ArgumentOutOfRangeException">If a tolerance is negative</exception>
        /// <exception cref="ArgumentException">If <paramref name="b"/> is zero</exception>
        public static double DivisionTolerance(double a, double b, double ta, double tb, double minTolerance = 0)
        {
            return Math.Sqrt(DivisionSquareTolerance(a, b, ta, tb, minTolerance));
        }

        private static void CheckTolerance(double tolerance, string name)
        {
            if (tolerance < 0 || double.IsNaN(tolerance))
                throw new ArgumentOutOfRangeException(name, tolerance, "Tolerance cannot be negative");
        }
    }
}
