using System;

namespace GPC.Utilities.Fem
{
	/// <summary>
	/// Funzioni di forma quadratiche e loro derivate per elemento triangolare a 6 nodi
	/// </summary>
	public static class QuadraticShapeFunctionsTri6
	{
		/// <summary> 
		/// Shape functions write in natural coordinates (csi and eta) - "A study of three - node triangular plate bending elements" - Batoz
		/// </summary> 
		/// <param name="index">Index of the shape function (from 1 to 6)</param> 
		/// <param name="csi">First natural coordinate</param> 
		/// <param name="eta">Second natural coordinate</param> 
		/// <returns>The shape function</returns> 
		/// <exception cref="ArgumentException"></exception>
		public static double NaturalShapeFunction(int index, double csi, double eta)
		{
			switch (index)
			{
				case 1:
					return csi * (2.0 * csi - 1.0);
				case 2:
					return eta * (2.0 * eta - 1.0);
				case 3:
					return (1.0 - csi - eta) * (2.0 * (1.0 - csi - eta) - 1.0);
				case 4:
					return 4.0 * csi * eta;
				case 5:
					return 4.0 * eta * (1.0 - csi - eta);
				case 6:
					return 4.0 * csi * (1.0 - csi - eta);
				default:
					throw new ArgumentException("indice da 1 a 6");
			}
		}

		/// <summary> 
		/// The derivative of the shape functions respect to the natural coordinates Eta
		/// </summary> 
		/// <param name="index">Index of the shape function (from 1 to 6)</param> 
		/// <param name="csi">First natural coordinate</param> 
		/// <param name="eta">Second natural coordinate</param> 
		/// <returns>The derivative of the shape function</returns>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static double DNdEta(int index, double csi, double eta)
		{
			switch (index)
			{
				case 1:
					return 0.0;
				case 2:
					return 4.0 * eta - 1.0;
				case 3:
					return 4.0 * csi + 4.0 * eta - 3.0;
				case 4:
					return 4.0 * csi;
				case 5:
					return 4.0 - 8.0 * eta - 4.0 * csi;
				case 6:
					return -4.0 * csi;
				default:
					throw new ArgumentException("indice da 1 a 6");
			}
		}

		/// <summary> 
		/// The derivative of the shape functions respect to the natural coordinates Csi
		/// </summary> 
		/// <param name="index">Index of the shape function (from 1 to 6)</param> 
		/// <param name="csi">First natural coordinate</param> 
		/// <param name="eta">Second natural coordinate</param> 
		/// <returns>The derivative of the shape function</returns>
		/// <exception cref="ArgumentException"></exception>
		public static double DNdCsi(int index, double csi, double eta)
		{
			switch (index)
			{
				case 1:
					return 4.0 * csi - 1.0;
				case 2:
					return 0.0;
				case 3:
					return 4.0 * csi + 4.0 * eta - 3.0;
				case 4:
					return 4.0 * eta;
				case 5:
					return -4.0 * eta;
				case 6:
					return 4.0 - 8.0 * csi - 4.0 * eta;
				default:
					throw new ArgumentException("indice da 1 a 6");
			}
		}
	}
}
