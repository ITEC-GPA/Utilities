using System;

namespace GPC.Utilities.Fem
{
	public class QuadraticShapeFunctionLine3
	{
		/// <summary>
		/// Linear Shape Function for Line
		/// </summary>
		/// <param name="index">Index of the shape function (from 1 to 3)</param>
		/// <param name="csi">Natural coordinate</param> 
		/// <returns></returns>
		public static double NaturalShapeFunction(int index, double csi)
		{
			switch (index)
			{
				case 1:
					return -0.5 * csi * (1 - csi);
				case 2:
					return 0.5 * csi * (1 + csi);
				case 3:
					return 1.0 - Math.Pow(csi, 2.0);

				default:
					throw new ArgumentException("indice da 1 a 3");
			}
		}

		/// <summary> 
		/// The derivative of the shape functions respect to the natural coordinates csi 
		/// </summary> 
		/// <param name="index">Index of the shape function (from 1 to 2)</param>
		/// <param name="csi">Natural coordinate</param> 
		/// <returns>The derivative of the shape function</returns>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static double DNdCsi(int index, double csi)
		{
			switch (index)
			{
				case 1:
					return -0.5 + csi;
				case 2:
					return +0.5 + csi;
				case 3:
					return -2.0 * csi;
				default:
					throw new ArgumentException("indice da 1 a 3");
			}
		}
	}
}
