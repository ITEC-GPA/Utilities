using System;

namespace GPC.Utilities.Fem
{
	/// <summary>
	/// Funzioni di forma lineare e loro derivate per elemento quadrilatero a 4 nodi
	/// </summary>
	public static class LinearShapeFunctionQuad4
	{
		/// <summary>
		/// Linear Shape Function for Quad4
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
					return 0.25 * (1.0 - csi) * (1.0 - eta);
				case 2:
					return 0.25 * (1.0 + csi) * (1.0 - eta);
				case 3:
					return 0.25 * (1.0 + csi) * (1.0 + eta);
				case 4:
					return 0.25 * (1.0 - csi) * (1.0 + eta);
				default:
					throw new ArgumentException("indice da 1 a 4");
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
					return (eta - 1.0) / 4.0;
				case 2:
					return (1.0 - eta) / 4.0;
				case 3:
					return (eta + 1.0) / 4.0;
				case 4:
					return (-eta - 1.0) / 4.0;
				default:
					throw new ArgumentException("indice da 1 a 4");
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
					return (csi - 1.0) / 4.0;
				case 2:
					return (-csi - 1.0) / 4.0;
				case 3:
					return (csi + 1.0) / 4.0;
				case 4:
					return (1.0 - csi) / 4.0;
				default:
					throw new ArgumentException("indice da 1 a 4");
			}
		}

		/// <summary>
		/// Partial derivative of shape function respect to variable Csi
		/// </summary>
		/// <param name="index">Index of the shape function (from 1 to 3)</param> 
		/// <param name="eta">Second natural coordinate</param> 
		/// <returns></returns>
		public static double DNdCsi(int index, double eta)
		{
			switch (index)
			{
				case 1:
					return (eta - 1.0) / 4.0;
				case 2:
					return (1.0 - eta) / 4.0;
				case 3:
					return (eta + 1.0) / 4.0;
				case 4:
					return (-eta - 1.0) / 4.0;
				default:
					throw new ArgumentException("indice da 1 a 4");
			}
		}

		/// <summary>
		/// Partial derivative of shape function respect to variable Eta
		/// </summary>
		/// <param name="index">Index of the shape function (from 1 to 3)</param> 
		/// <param name="csi">First natural coordinate</param>         
		/// <returns></returns>
		public static double DNdEta(int index, double csi)
		{
			switch (index)
			{
				case 1:
					return (csi - 1.0) / 4.0;
				case 2:
					return (-csi - 1.0) / 4.0;
				case 3:
					return (csi + 1.0) / 4.0;
				case 4:
					return (1.0 - csi) / 4.0;
				default:
					throw new ArgumentException("indice da 1 a 4");
			}
		}
	}
}
