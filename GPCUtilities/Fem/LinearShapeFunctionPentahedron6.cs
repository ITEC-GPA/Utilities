using System;
using System.Collections.Generic;

namespace GPC.Utilities.Fem
{
    /// <summary>
    /// Funzioni di forma lineare e loro derivate per elemento pentaedro (prisma a base triangolare) a 6 nodi.
    /// Stessa convenzione di <see cref="LinearShapeFunctionsTri3"/>: nodo 1 in (csi=1, eta=0), nodo 2 in (csi=0, eta=1), nodo 3 in (csi=0, eta=0);
    /// nodi 1-3 sulla faccia zeta=-1, nodi 4-6 sulla faccia zeta=+1
    /// </summary>
    public static class LinearShapeFunctionPentahedron6
    {
        private static readonly Dictionary<int, (double, double, double)> _points = new Dictionary<int, (double, double, double)>(6) {
            { 1, (+1.0, +0.0, -1.0) },
            { 2, (+0.0, +1.0, -1.0) },
            { 3, (+0.0, +0.0, -1.0) },
            { 4, (+1.0, +0.0, +1.0) },
            { 5, (+0.0, +1.0, +1.0) },
            { 6, (+0.0, +0.0, +1.0) },
        };

        #region Shape Function

        /// <summary>
        /// Linear Shape Function for Pentahedron6
        /// </summary>
        /// <param name="index"></param>
        /// <param name="csi"></param>
        /// <param name="eta"></param>
        /// <param name="zeta"></param>
        /// <returns></returns>
        public static double NaturalShapeFunction(int index, double csi, double eta, double zeta)
        {
            switch (index)
            {
                case 1:
                    return 0.50 * (1.0 - zeta) * csi;
                case 2:
                    return 0.50 * (1.0 - zeta) * eta;
                case 3:
                    return 0.50 * (1.0 - zeta) * (1.0 - csi - eta);
                case 4:
                    return 0.50 * (1.0 + zeta) * csi;
                case 5:
                    return 0.50 * (1.0 + zeta) * eta;
                case 6:
                    return 0.50 * (1.0 + zeta) * (1.0 - csi - eta);
                default:
                    throw new ArgumentException("indice da 1 a 6");
            }
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
            switch (index)
            {
                case 1:
                    return +0.5 * (1.0 - zeta);
                case 2:
                    return +0.0;
                case 3:
                    return -0.5 * (1.0 - zeta);
                case 4:
                    return +0.5 * (1.0 + zeta);
                case 5:
                    return +0.0;
                case 6:
                    return -0.5 * (1.0 + zeta);
                default:
                    throw new ArgumentException("indice da 1 a 6");
            }
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
            switch (index)
            {
                case 1:
                    return +0.0;
                case 2:
                    return +0.5 * (1.0 - zeta);
                case 3:
                    return -0.5 * (1.0 - zeta);
                case 4:
                    return +0.0;
                case 5:
                    return +0.5 * (1.0 + zeta);
                case 6:
                    return -0.5 * (1.0 + zeta);
                default:
                    throw new ArgumentException("indice da 1 a 6");
            }
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
            switch (index)
            {
                case 1:
                    return -0.5 * csi;
                case 2:
                    return -0.5 * eta;
                case 3:
                    return -0.5 * (1 - csi - eta);
                case 4:
                    return +0.5 * csi;
                case 5:
                    return +0.5 * eta;
                case 6:
                    return +0.5 * (1 - csi - eta);
                default:
                    throw new ArgumentException("indice da 1 a 6");
            }
        }

        #endregion
    }
}
