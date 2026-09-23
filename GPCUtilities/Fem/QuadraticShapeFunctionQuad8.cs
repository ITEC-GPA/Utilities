using System;

namespace GPC.Utilities.Fem
{
	public static class QuadraticShapeFunctionQuad8
	{
		/// <summary>
		/// Quadratic Shape Function for Quad8
		/// </summary>
		/// <param name="index">Index of the shape function (from 1 to 3)</param> 
		/// <param name="csi">First natural coordinate</param> 
		/// <param name="eta">Second natural coordinate</param> 
		/// <returns></returns>
		public static double NaturalShapeFunction(int index, double csi, double eta)
		{
			switch (index)
			{
				case 1:
					return -0.25 * (1.0 - csi) * (1.0 - eta) * (1.0 + csi + eta);
				case 2:
					return -0.25 * (1.0 + csi) * (1.0 - eta) * (1.0 - csi + eta);
				case 3:
					return -0.25 * (1.0 + csi) * (1.0 + eta) * (1.0 - csi - eta);
				case 4:
					return -0.25 * (1.0 - csi) * (1.0 + eta) * (1.0 + csi - eta);
				case 5:
					return 0.5 * (1.0 - csi) * (1.0 + csi) * (1.0 - eta);
				case 6:
					return 0.5 * (1.0 + csi) * (1.0 + eta) * (1.0 - eta);
				case 7:
					return 0.5 * (1.0 - csi) * (1.0 + csi) * (1.0 + eta);
				case 8:
					return 0.5 * (1.0 - csi) * (1.0 + eta) * (1.0 - eta);
				default:
					throw new ArgumentException("indice da 1 a 8");
			}
		}

		/// <summary>
		/// Partial derivative of shape function respect to variable Csi
		/// </summary>
		/// <param name="index">Index of the shape function (from 1 to 3)</param> 
		/// <param name="csi">First natural coordinate</param> 
		/// <param name="eta">Second natural coordinate</param> 
		/// <returns></returns>
		public static double DNdCsi(int index, double csi, double eta)
		{
			switch (index)
			{
				case 1:
					return 0.25 * (2.0 * csi + eta) * (1.0 - eta);
				case 2:
					return 0.25 * (2.0 * csi - eta) * (1.0 - eta);
				case 3:
					return 0.25 * (2.0 * csi + eta) * (1.0 + eta);
				case 4:
					return 0.25 * (2.0 * csi - eta) * (1.0 + eta);
				case 5:
					return csi * (eta - 1.0);
				case 6:
					return -0.5 * (eta - 1.0) * (eta + 1.0);
				case 7:
					return -csi * (1.0 + eta);
				case 8:
					return 0.5 * (eta - 1.0) * (eta + 1.0);
				default:
					throw new ArgumentException("indice da 1 a 8");
			}
		}

		/// <summary>
		/// Partial derivative of shape function respect to variable Eta
		/// </summary>
		/// <param name="index">Index of the shape function (from 1 to 3)</param> 
		/// <param name="csi">First natural coordinate</param> 
		/// <param name="eta">Second natural coordinate</param> 
		/// <returns></returns>
		public static double DNdEta(int index, double csi, double eta)
		{
			switch (index)
			{
				case 1:
					return 0.25 * (2.0 * eta + csi) * (1.0 - csi);
				case 2:
					return 0.25 * (2.0 * eta - csi) * (1.0 + csi);
				case 3:
					return 0.25 * (2.0 * eta + csi) * (1.0 + csi);
				case 4:
					return 0.25 * (2.0 * eta - csi) * (1.0 - csi);
				case 5:
					return 0.5 * (csi - 1.0) * (csi + 1.0);
				case 6:
					return -eta * (1.0 + csi);
				case 7:
					return -0.5 * (csi - 1.0) * (csi + 1.0);
				case 8:
					return eta * (csi - 1.0);
				default:
					throw new ArgumentException("indice da 1 a 8");
			}
		}
	}
}
