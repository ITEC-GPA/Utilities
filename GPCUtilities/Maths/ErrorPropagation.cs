using System;

namespace GPC.Utilities.Maths
{
    public static class ErrorPropagation
    {
        /*
        https://it.wikipedia.org/wiki/Propagazione_degli_errori#:~:text=In%20fisica%2C%20per%20propagazione%20degli,ad%20una%20funzione%20di%20esso
        */

        /// <summary>
        /// Calculate the tolerance of the result of the sum of two terms
        /// </summary>
        /// <param name="ta">Tolerance or uncertainty of the first term of the sum</param>
        /// <param name="tb">Tolerance or uncertainty of the second term of the sum</param>
        /// <returns>The value of tolerance</returns>
        public static double SumSquareTolerance(double ta, double tb)
        {
            if (ta == 0 || tb == 0)
                throw new ArgumentNullException("Tolerance can't be zero");

            double tol = (Math.Pow(ta, 2) + Math.Pow(tb, 2));
            return tol;
        }

		/// <summary>
		/// Calculate the tolerance of the result of the sum of two terms
		/// </summary>
		/// <param name="ta">Tolerance or uncertainty of the first term of the sum</param>
		/// <param name="tb">Tolerance or uncertainty of the second term of the sum</param>
		/// <returns>The value of tolerance</returns>
		public static double SumTolerance(double ta, double tb)
		{
            return Math.Sqrt(SumSquareTolerance(ta, tb));
		}

		/// <summary>
		/// Calculate the tolerance of the result of the product of two terms
		/// </summary>
		/// <param name="a">First term of the product</param>
		/// <param name="b">Second term of the product</param>
		/// <param name="ta">Tolerance or uncertainty of the first term of the product</param>
		/// <param name="tb">Tolerance or uncertainty of the second term of the product</param>
		/// <param name="minTolerance">Minimal intrinsic tolerance equal to the product of the three single tolerances</param>
		/// <returns>The value of tolerance</returns>
		public static double ProductSquareTolerance(double a, double b, double ta, double tb, double minTolerance = 0)
        {
            if (ta == 0 || tb == 0)
                throw new ArgumentNullException("Tolerance can't be zero");

            minTolerance = ta * tb;
            double tol = (Math.Pow(tb, 2) * Math.Pow(a, 2) + Math.Pow(ta, 2) * Math.Pow(b, 2));
            tol = tol < minTolerance ? minTolerance : tol;
            return tol;
        }

		/// <summary>
		/// Calculate the tolerance of the result of the product of two terms
		/// </summary>
		/// <param name="a">First term of the product</param>
		/// <param name="b">Second term of the product</param>
		/// <param name="ta">Tolerance or uncertainty of the first term of the product</param>
		/// <param name="tb">Tolerance or uncertainty of the second term of the product</param>
		/// <param name="minTolerance">Minimal intrinsic tolerance equal to the product of the three single tolerances</param>
		/// <returns>The value of tolerance</returns>
		public static double ProductTolerance(double a, double b, double ta, double tb, double minTolerance = 0)
        {
            return Math.Sqrt(ProductSquareTolerance(a, b, ta, tb, minTolerance));
		}

		/// <summary>
		/// Calculate the tolerance of the result of the product of two terms
		/// </summary>
		/// <param name="a">First term of the product</param>
		/// <param name="b">Second term of the product</param>
		/// <param name="c">Third term of the product</param>
		/// <param name="ta">Tolerance or uncertainty of the first term of the product</param>
		/// <param name="tb">Tolerance or uncertainty of the second term of the product</param>
		/// <param name="tc">Tolerance or uncertainty of the third term of the product</param>
		/// <param name="minTolerance">Minimal intrinsic tolerance equal to the product of the three single tolerances</param>
		/// <returns>The value of tolerance</returns>
		public static double ProductSquareTolerance(double a, double b, double c, double ta, double tb, double tc, double minTolerance = 0)
        {
            if (ta == 0 || tb == 0 || tc == 0)
                throw new ArgumentNullException("Tolerance can't be zero");

            minTolerance = ta * tb * tc;
            double tol = (Math.Pow(tb, 2) * Math.Pow(a * c, 2) + Math.Pow(ta, 2) * Math.Pow(b * c, 2) + Math.Pow(tc, 2) * Math.Pow(a * c, 2));
            tol = tol < minTolerance ? minTolerance : tol;
            return tol;
        }

		/// <summary>
		/// Calculate the tolerance of the result of the product of two terms
		/// </summary>
		/// <param name="a">First term of the product</param>
		/// <param name="b">Second term of the product</param>
		/// <param name="c">Third term of the product</param>
		/// <param name="ta">Tolerance or uncertainty of the first term of the product</param>
		/// <param name="tb">Tolerance or uncertainty of the second term of the product</param>
		/// <param name="tc">Tolerance or uncertainty of the third term of the product</param>
		/// <param name="minTolerance">Minimal intrinsic tolerance equal to the product of the three single tolerances</param>
		/// <returns>The value of tolerance</returns>
		public static double ProductTolerance(double a, double b, double c, double ta, double tb, double tc, double minTolerance = 0)
		{
            return Math.Sqrt(ProductSquareTolerance(a, b, c, ta, tb, tc, minTolerance));
		}

		/// <summary>
		/// Calculate the tolerance of the result of the product of two terms in case there aren't two explicit terms of product or in case of squaring (exponentiation of power 2)
		/// </summary>
		/// <param name="t">Tolerance</param>
		/// <returns>The value of tolerance</returns>
		public static double DefaultProductSquareTolerance(double t)
        {
            if (t == 0)
                throw new ArgumentNullException("Tolerance can't be zero");

            double tol = (Math.Pow(t, 2) * 1.4142);
            return tol;
        }

		/// <summary>
		/// Calculate the tolerance of the result of the product of two terms in case there aren't two explicit terms of product or in case of squaring (exponentiation of power 2)
		/// </summary>
		/// <param name="t">Tolerance</param>
		/// <returns>The value of tolerance</returns>
		public static double DefaultProductTolerance(double t)
		{
			return Math.Sqrt(DefaultProductSquareTolerance(t));
		}

		/// <summary>
		/// Calculate the tolerance of the result of the division of two terms
		/// </summary>
		/// <param name="a">First term of the division</param>
		/// <param name="b">Second term of the division</param>
		/// <param name="ta">Tolerance or uncertainty of the first term of the division</param>
		/// <param name="tb">Tolerance or uncertainty of the second term of the division</param>
		/// <param name="minTolerance">Minimal intrinsic tolerance equal to the product of the three single tolerances</param>
		/// <returns>The value of tolerance</returns>
		public static double DivisionSquareTolerance(double a, double b, double ta, double tb, double minTolerance = 0)
        {
            if (ta == 0 || tb == 0)
                throw new ArgumentNullException("Tolerance can't be zero");

            minTolerance = ta * tb;
            double tol;
            try
            {
                tol = (Math.Pow(ta, 2) / Math.Pow(b, 2)) + (Math.Pow(a, 2) * Math.Pow(tb, 2)) / Math.Pow(b, 4);
            }
            catch
            {
                return minTolerance;
            }
            tol = tol < minTolerance ? minTolerance : tol;
            return tol;
        }

		/// <summary>
		/// Calculate the tolerance of the result of the division of two terms
		/// </summary>
		/// <param name="a">First term of the division</param>
		/// <param name="b">Second term of the division</param>
		/// <param name="ta">Tolerance or uncertainty of the first term of the division</param>
		/// <param name="tb">Tolerance or uncertainty of the second term of the division</param>
		/// <param name="minTolerance">Minimal intrinsic tolerance equal to the product of the three single tolerances</param>
		/// <returns>The value of tolerance</returns>
		public static double DivisionTolerance(double a, double b, double ta, double tb, double minTolerance = 0)
		{
			return Math.Sqrt(DivisionSquareTolerance(a, b, ta, tb, minTolerance));
		}
	}
}
