using System.Collections.Generic;

namespace GPC.Utilities.Fem
{
	/// <summary>
	/// Funzioni di forma lineare e loro derivate per elemento quadrilatero a 4 nodi
	/// </summary>
	public static class LinearShapeFunctionHexaedron8
	{
		private static readonly Dictionary<int, (double, double, double)> _points = new Dictionary<int, (double, double, double)>(8) {
			{ 1, (-1.0, -1.0, -1.0) },
			{ 2, (+1.0, -1.0, -1.0) },
			{ 3, (+1.0, +1.0, -1.0) },
			{ 4, (-1.0, +1.0, -1.0) },

			{ 5, (-1.0, -1.0, +1.0) },
			{ 6, (+1.0, -1.0, +1.0) },
			{ 7, (+1.0, +1.0, +1.0) },
			{ 8, (-1.0, +1.0, +1.0) }
		};

		#region Shape Function

		/// <summary>
		/// Linear Shape Function for Quad4
		/// </summary>
		/// <param name="index"></param>
		/// <param name="csi"></param>
		/// <param name="eta"></param>
		/// <param name="zeta"></param>
		/// <returns></returns>
		public static double NaturalShapeFunction(int index, double csi, double eta, double zeta)
		{
			double csiI = _points[index].Item1;
			double etaI = _points[index].Item2;
			double zetaI = _points[index].Item3;

			return 1.0 / 8.0 * (1.0 + csiI * csi) * (1.0 + etaI * eta) * (1.0 + zetaI * zeta);
		}

		/// <summary>
		/// Derivate parziali delle funzioni di forma rispetto a csi
		/// </summary>
		/// <param name="index"></param>
		/// <param name="csi"></param>
		/// <param name="eta"></param>
		/// <param name="zeta"></param>
		/// <returns></returns>
		public static double DNdCsi(int index, double csi, double eta, double zeta)
		{
			double csiI = _points[index].Item1;
			double etaI = _points[index].Item2;
			double zetaI = _points[index].Item3;

			return 1.0 / 8.0 * csiI * (1.0 + etaI * eta) * (1.0 + zetaI * zeta);
		}

		/// <summary>
		/// Derivate parziali delle funzioni di forma rispetto a eta
		/// </summary>
		/// <param name="index"></param>
		/// <param name="csi"></param>
		/// <param name="eta"></param>
		/// <param name="zeta"></param>
		/// <returns></returns>
		public static double DNdEta(int index, double csi, double eta, double zeta)
		{
			double csiI = _points[index].Item1;
			double etaI = _points[index].Item2;
			double zetaI = _points[index].Item3;

			return 1.0 / 8.0 * etaI * (1.0 + csiI * csi) * (1.0 + zetaI * zeta);
		}

		/// <summary>
		/// Derivate parziali delle funzioni di forma rispetto a zeta
		/// </summary>
		/// <param name="index"></param>
		/// <param name="csi"></param>
		/// <param name="eta"></param>
		/// <param name="zeta"></param>
		/// <returns></returns>
		public static double DNdZeta(int index, double csi, double eta, double zeta)
		{
			double csiI = _points[index].Item1;
			double etaI = _points[index].Item2;
			double zetaI = _points[index].Item3;

			return 1.0 / 8.0 * zetaI * (1.0 + csiI * csi) * (1.0 + etaI * eta);
		}

		#endregion
	}
}
