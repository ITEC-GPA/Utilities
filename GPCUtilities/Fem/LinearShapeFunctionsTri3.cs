using System;

namespace GPC.Utilities.Fem
{
	/// <summary>
	/// Funzioni di forma lineare e loro derivate per elemento triangolare a 3 nodi
	/// </summary>
	public static class LinearShapeFunctionsTri3
	{
		/// <summary> 
		/// Shape functions write in natural coordinates (csi and eta) 
		/// Attention: First node of triangle in "bottom right" (csi=1, eta=0), second "top left" (csi=0,eta=1), third "bottom left" (csi=0, eta=0)
		/// </summary> 
		/// <param name="index">Index of the shape function (from 1 to 3)</param> 
		/// <param name="csi">First natural coordinate</param> 
		/// <param name="eta">Second natural coordinate</param> 
		/// <returns>The shape function</returns> 
		/// <exception cref="ArgumentException"></exception>
		public static double NaturalShapeFunction(int index, double csi, double eta)
		{
			switch (index)
			{
				case 1:
					return csi;
				case 2:
					return eta;
				case 3:
					return 1.0 - csi - eta;
				default:
					throw new ArgumentException("indice da 1 a 3");
			}
		}

		/// <summary> 
		/// Partial derivative of shape function respect to variable Csi
		/// </summary> 
		/// <param name="index">Index of the shape function (from 1 to 3)</param>
		/// <param name="csi">First natural coordinate</param> 
		/// <param name="eta">Second natural coordinate</param> 
		/// <returns>The derivative of the shape function</returns>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static double DNdCsi(int index, double csi, double eta)
		{
			switch (index)
			{
				case 1:
					return 1.0;
				case 2:
					return 0.0;
				case 3:
					return -1.0;
				default:
					throw new ArgumentException("indice da 1 a 3");
			}
		}

		/// <summary> 
		/// Partial derivative of shape function respect to variable Eta
		/// </summary> 
		/// <param name="index">Index of the shape function (from 1 to 3)</param>
		/// <param name="csi">First natural coordinate</param> 
		/// <param name="eta">Second natural coordinate</param> 
		/// <returns>The derivative of the shape function</returns> >
		/// <exception cref="ArgumentException"></exception>
		public static double DNdEta(int index, double csi, double eta)
		{
			switch (index)
			{
				case 1:
					return 0.0;
				case 2:
					return 1.0;
				case 3:
					return -1.0;
				default:
					throw new ArgumentException("indice da 1 a 3");
			}
		}

		/// <summary> 
		/// Partial derivative of shape function respect to variable Csi
		/// </summary> 
		/// <param name="index">Index of the shape function (from 1 to 3)</param>
		/// <returns>The derivative of the shape function</returns>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static double DNdCsi(int index)
		{
			switch (index)
			{
				case 1:
					return 1.0;
				case 2:
					return 0.0;
				case 3:
					return -1.0;
				default:
					throw new ArgumentException("indice da 1 a 3");
			}
		}

		/// <summary> 
		/// Partial derivative of shape function respect to variable Eta
		/// </summary> 
		/// <param name="index">Index of the shape function (from 1 to 3)</param>
		/// <returns>The derivative of the shape function</returns> >
		/// <exception cref="ArgumentException"></exception>
		public static double DNdEta(int index)
		{
			switch (index)
			{
				case 1:
					return 0.0;
				case 2:
					return 1.0;
				case 3:
					return -1.0;
				default:
					throw new ArgumentException("indice da 1 a 3");
			}
		}

		/// <summary> 
		/// Calculate the shape function of point <paramref name="x"/>, <paramref name="y"/> in global coordinates (x, y, z) 
		/// </summary> 
		/// <param name="index">Index of the shape function (from 1 to 3)</param> 
		/// <param name="x">X coordinate of the parametric point</param> 
		/// <param name="y">Y coordinate of the parametric point</param> 
		/// <param name="x1">X coordinate of the first vertex of the element</param> 
		/// <param name="y1">Y coordinate of the first vertex of the element</param> 
		/// <param name="x2">X coordinate of the second vertex of the element</param> 
		/// <param name="y2">Y coordinate of the second vertex of the element</param> 
		/// <param name="x3">X coordinate of the third vertex of the element</param> 
		/// <param name="y3">Y coordinate of the third vertex of the element</param> 
		/// <returns>The shape function</returns> 
		public static double LocalShapeFunction(int index, double x, double y, double x1, double y1, double x2, double y2, double x3, double y3)
		{
			double area = Math.Abs(x1 * y2 + y1 * x3 + x2 * y3 - x3 * y2 - y3 * x1 - x2 * y1) / 2;

			switch (index)
			{
				case 1:
					return (1.0 / (2.0 * area) * ((x2 * y3 - x3 * y2) + (y2 - y3) * x + (x3 - x2) * y));
				case 2:
					return (1.0 / (2.0 * area) * ((x3 * y1 - x1 * y3) + (y3 - y1) * x + (x1 - x3) * y));
				case 3:
					return (1.0 / (2.0 * area) * ((x1 * y2 - x2 * y1) + (y1 - y2) * x + (x2 - x1) * y));
				default:
					throw new ArgumentException("indice da 1 a 3");
			}
		}

		/// <summary>
		/// Calculate the shape functions of point <paramref name="x"/>, <paramref name="y"/> in global coordinates (x, y, z) 
		/// </summary>
		/// <param name="x">X coordinate of the parametric point</param> 
		/// <param name="y">Y coordinate of the parametric point</param> 
		/// <param name="x1">X coordinate of the first vertex of the element</param> 
		/// <param name="y1">Y coordinate of the first vertex of the element</param> 
		/// <param name="x2">X coordinate of the second vertex of the element</param> 
		/// <param name="y2">Y coordinate of the second vertex of the element</param> 
		/// <param name="x3">X coordinate of the third vertex of the element</param> 
		/// <param name="y3">Y coordinate of the third vertex of the element</param> 
		/// <param name="n1">First shape function N1</param>
		/// <param name="n2">Second shape function N2</param>
		/// <param name="n3">Third shape function N3</param>
		public static void LocalShapeFunction(double x, double y, double x1, double y1, double x2, double y2, double x3, double y3,
			out double n1, out double n2, out double n3)
		{
			double area = Math.Abs(x1 * y2 + y1 * x3 + x2 * y3 - x3 * y2 - y3 * x1 - x2 * y1) / 2;

			n1 = (1.0 / (2.0 * area) * ((x2 * y3 - x3 * y2) + (y2 - y3) * x + (x3 - x2) * y));
			n2 = (1.0 / (2.0 * area) * ((x3 * y1 - x1 * y3) + (y3 - y1) * x + (x1 - x3) * y));
			n3 = (1.0 / (2.0 * area) * ((x1 * y2 - x2 * y1) + (y1 - y2) * x + (x2 - x1) * y));
		}
	}
}
