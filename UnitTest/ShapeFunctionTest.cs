using System;
using GPC.Utilities.Fem;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GPC.Utilities.UnitTest
{
    /// <summary>
    /// The analytical derivatives of the shape functions are compared with the finite differences of the shape functions themselves
    /// </summary>
    [TestClass]
    public class ShapeFunctionTest
    {
        private const double H = 1e-6;
        private const double Tol = 1e-7;

        private static readonly double[] Samples = { -0.7, -0.2, 0.0, 0.15, 0.4, 0.8 };

        private static void AssertDerivative2D(int count, Func<int, double, double, double> n,
            Func<int, double, double, double> dNdCsi, Func<int, double, double, double> dNdEta)
        {
            foreach (double csi in Samples)
                foreach (double eta in Samples)
                {
                    double sum = 0;
                    for (int i = 1; i <= count; i++)
                    {
                        sum += n(i, csi, eta);
                        double fdCsi = (n(i, csi + H, eta) - n(i, csi - H, eta)) / (2 * H);
                        double fdEta = (n(i, csi, eta + H) - n(i, csi, eta - H)) / (2 * H);
                        Assert.AreEqual(fdCsi, dNdCsi(i, csi, eta), Tol, $"dN{i}/dCsi at ({csi}, {eta})");
                        Assert.AreEqual(fdEta, dNdEta(i, csi, eta), Tol, $"dN{i}/dEta at ({csi}, {eta})");
                    }
                    Assert.AreEqual(1.0, sum, 1e-12, "Partition of unity");
                }
        }

        private static void AssertDerivative3D(int count, Func<int, double, double, double, double> n,
            Func<int, double, double, double, double> dNdCsi, Func<int, double, double, double, double> dNdEta, Func<int, double, double, double, double> dNdZeta)
        {
            foreach (double csi in Samples)
                foreach (double eta in Samples)
                    foreach (double zeta in Samples)
                    {
                        double sum = 0;
                        for (int i = 1; i <= count; i++)
                        {
                            sum += n(i, csi, eta, zeta);
                            double fdCsi = (n(i, csi + H, eta, zeta) - n(i, csi - H, eta, zeta)) / (2 * H);
                            double fdEta = (n(i, csi, eta + H, zeta) - n(i, csi, eta - H, zeta)) / (2 * H);
                            double fdZeta = (n(i, csi, eta, zeta + H) - n(i, csi, eta, zeta - H)) / (2 * H);
                            Assert.AreEqual(fdCsi, dNdCsi(i, csi, eta, zeta), Tol, $"dN{i}/dCsi at ({csi}, {eta}, {zeta})");
                            Assert.AreEqual(fdEta, dNdEta(i, csi, eta, zeta), Tol, $"dN{i}/dEta at ({csi}, {eta}, {zeta})");
                            Assert.AreEqual(fdZeta, dNdZeta(i, csi, eta, zeta), Tol, $"dN{i}/dZeta at ({csi}, {eta}, {zeta})");
                        }
                        Assert.AreEqual(1.0, sum, 1e-12, "Partition of unity");
                    }
        }

        [TestMethod]
        public void Pentahedron6Derivatives()
        {
            AssertDerivative3D(6, LinearShapeFunctionPentahedron6.NaturalShapeFunction, LinearShapeFunctionPentahedron6.DNdCsi,
                LinearShapeFunctionPentahedron6.DNdEta, LinearShapeFunctionPentahedron6.DNdZeta);
        }

        [TestMethod]
        public void Pentahedron6NodalValues()
        {
            // node 1 (csi=1, eta=0), node 2 (csi=0, eta=1), node 3 (csi=0, eta=0); nodes 1-3 on zeta=-1, nodes 4-6 on zeta=+1
            double[,] nodes = { { 1, 0, -1 }, { 0, 1, -1 }, { 0, 0, -1 }, { 1, 0, 1 }, { 0, 1, 1 }, { 0, 0, 1 } };
            for (int j = 0; j < 6; j++)
                for (int i = 1; i <= 6; i++)
                    Assert.AreEqual(i == j + 1 ? 1.0 : 0.0, LinearShapeFunctionPentahedron6.NaturalShapeFunction(i, nodes[j, 0], nodes[j, 1], nodes[j, 2]), 1e-12);
        }

        [TestMethod]
        public void Hexaedron8Derivatives()
        {
            AssertDerivative3D(8, LinearShapeFunctionHexaedron8.NaturalShapeFunction, LinearShapeFunctionHexaedron8.DNdCsi,
                LinearShapeFunctionHexaedron8.DNdEta, LinearShapeFunctionHexaedron8.DNdZeta);
        }

        [TestMethod]
        public void Quad4Derivatives()
        {
            AssertDerivative2D(4, LinearShapeFunctionQuad4.NaturalShapeFunction, LinearShapeFunctionQuad4.DNdCsi, LinearShapeFunctionQuad4.DNdEta);
        }

        [TestMethod]
        public void Quad8Derivatives()
        {
            AssertDerivative2D(8, QuadraticShapeFunctionQuad8.NaturalShapeFunction, QuadraticShapeFunctionQuad8.DNdCsi, QuadraticShapeFunctionQuad8.DNdEta);
        }

        [TestMethod]
        public void Tri3Derivatives()
        {
            AssertDerivative2D(3, LinearShapeFunctionsTri3.NaturalShapeFunction, LinearShapeFunctionsTri3.DNdCsi, LinearShapeFunctionsTri3.DNdEta);
        }

        [TestMethod]
        public void Tri6Derivatives()
        {
            AssertDerivative2D(6, QuadraticShapeFunctionsTri6.NaturalShapeFunction, QuadraticShapeFunctionsTri6.DNdCsi, QuadraticShapeFunctionsTri6.DNdEta);
        }

        [TestMethod]
        public void Line2And3Derivatives()
        {
            foreach (double csi in Samples)
            {
                for (int i = 1; i <= 2; i++)
                {
                    double fd = (LinearShapeFunctionsLine2.NaturalShapeFunction(i, csi + H) - LinearShapeFunctionsLine2.NaturalShapeFunction(i, csi - H)) / (2 * H);
                    Assert.AreEqual(fd, LinearShapeFunctionsLine2.DNdCsi(i, csi), Tol);
                }
                for (int i = 1; i <= 3; i++)
                {
                    double fd = (QuadraticShapeFunctionLine3.NaturalShapeFunction(i, csi + H) - QuadraticShapeFunctionLine3.NaturalShapeFunction(i, csi - H)) / (2 * H);
                    Assert.AreEqual(fd, QuadraticShapeFunctionLine3.DNdCsi(i, csi), Tol);
                }
            }
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void Tri3LocalShapeFunctionBothOrientations(bool counterclockwise)
        {
            double x1 = 1, y1 = 2, x2 = 7, y2 = 3, x3 = 2, y3 = 8;
            if (!counterclockwise)
            {
                (x2, x3) = (x3, x2);
                (y2, y3) = (y3, y2);
            }

            double[] xs = { x1, x2, x3 };
            double[] ys = { y1, y2, y3 };

            for (int j = 0; j < 3; j++)
            {
                LinearShapeFunctionsTri3.LocalShapeFunction(xs[j], ys[j], x1, y1, x2, y2, x3, y3, out double n1, out double n2, out double n3);
                double[] n = { n1, n2, n3 };
                for (int i = 0; i < 3; i++)
                {
                    Assert.AreEqual(i == j ? 1.0 : 0.0, n[i], 1e-12);
                    Assert.AreEqual(n[i], LinearShapeFunctionsTri3.LocalShapeFunction(i + 1, xs[j], ys[j], x1, y1, x2, y2, x3, y3), 1e-12);
                }
            }

            LinearShapeFunctionsTri3.LocalShapeFunction((x1 + x2 + x3) / 3, (y1 + y2 + y3) / 3, x1, y1, x2, y2, x3, y3, out double c1, out double c2, out double c3);
            Assert.AreEqual(1.0 / 3.0, c1, 1e-12);
            Assert.AreEqual(1.0 / 3.0, c2, 1e-12);
            Assert.AreEqual(1.0 / 3.0, c3, 1e-12);
        }
    }
}
