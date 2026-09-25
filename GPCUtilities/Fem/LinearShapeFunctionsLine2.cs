using System;

namespace GPC.Utilities.Fem
{
	public class LinearShapeFunctionsLine2
	{
		/// <summary>
		/// Linear Shape Function for Line
		/// </summary>
		/// <param name="index">Index of the shape function (from 1 to 2)</param>
		/// <param name="csi">Natural coordinate</param> 
		/// <returns></returns>
		public static double NaturalShapeFunction(int index, double csi)
		{
			switch (index)
			{
				case 1:
					return 0.5 * (1 - csi);
				case 2:
					return 0.5 * (1 + csi);
				default:
					throw new ArgumentException("indice da 1 a 2");
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
					return -0.5;
				case 2:
					return +0.5;
				default:
					throw new ArgumentException("indice da 1 a 2");
			}
		}

	}
}
