using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPC.Utilities.Clone;
using GPC.Utilities.Enums;
using GPC.Utilities.Extensions;
using GPC.Utilities.Fem;
using GPC.Utilities.Maths;
using GPC.Utilities.Units;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static GPC.Utilities.Units.UnitsConvert;

namespace GPC.Utilities.UnitTest
{
    /// <summary>
    /// Shape functions: Kronecker property at the nodes and exact reproduction of the polynomials of the element space
    /// </summary>
    [TestClass]
    public class ShapeFunctionControlTest
    {
        private static readonly double[] Samples = { -0.9, -0.35, 0.0, 0.2, 0.65 };

        // natural coordinates of the nodes, in the order of the shape functions
        private static readonly double[][] Line3Nodes = { new[] { -1.0 }, new[] { 1.0 }, new[] { 0.0 } };
        private static readonly double[][] Quad4Nodes = { new[] { -1.0, -1 }, new[] { 1.0, -1 }, new[] { 1.0, 1 }, new[] { -1.0, 1 } };
        private static readonly double[][] Quad8Nodes = { new[] { -1.0, -1 }, new[] { 1.0, -1 }, new[] { 1.0, 1 }, new[] { -1.0, 1 },
            new[] { 0.0, -1 }, new[] { 1.0, 0 }, new[] { 0.0, 1 }, new[] { -1.0, 0 } };
        private static readonly double[][] Tri3Nodes = { new[] { 1.0, 0 }, new[] { 0.0, 1 }, new[] { 0.0, 0 } };
        private static readonly double[][] Tri6Nodes = { new[] { 1.0, 0 }, new[] { 0.0, 1 }, new[] { 0.0, 0 }, new[] { 0.5, 0.5 }, new[] { 0.0, 0.5 }, new[] { 0.5, 0 } };
        private static readonly double[][] Hexa8Nodes = { new[] { -1.0, -1, -1 }, new[] { 1.0, -1, -1 }, new[] { 1.0, 1, -1 }, new[] { -1.0, 1, -1 },
            new[] { -1.0, -1, 1 }, new[] { 1.0, -1, 1 }, new[] { 1.0, 1, 1 }, new[] { -1.0, 1, 1 } };

        private static void AssertKronecker(double[][] nodes, Func<int, double[], double> n)
        {
            for (int j = 0; j < nodes.Length; j++)
                for (int i = 1; i <= nodes.Length; i++)
                    Assert.AreEqual(i == j + 1 ? 1.0 : 0.0, n(i, nodes[j]), 1e-14, $"N{i} at node {j + 1}");
        }

        /// <summary>Σ N_i(x) f(x_i) = f(x) for every sample point</summary>
        private static void AssertReproduces(double[][] nodes, Func<int, double[], double> n, Func<double[], double> f, IEnumerable<double[]> points)
        {
            foreach (var p in points)
            {
                double sum = 0;
                for (int i = 1; i <= nodes.Length; i++)
                    sum += n(i, p) * f(nodes[i - 1]);
                Assert.AreEqual(f(p), sum, 1e-12, $"at ({string.Join(", ", p)})");
            }
        }

        private static IEnumerable<double[]> Grid2() => from a in Samples from b in Samples select new[] { a, b };
        private static IEnumerable<double[]> Triangle() => Grid2().Select(p => new[] { (p[0] + 1) / 2, (p[1] + 1) / 2 * (1 - (p[0] + 1) / 2) });

        [TestMethod]
        public void Line2NodesAndLinearField()
        {
            AssertKronecker(new[] { new[] { -1.0 }, new[] { 1.0 } }, (i, x) => LinearShapeFunctionsLine2.NaturalShapeFunction(i, x[0]));
            AssertReproduces(new[] { new[] { -1.0 }, new[] { 1.0 } }, (i, x) => LinearShapeFunctionsLine2.NaturalShapeFunction(i, x[0]),
                x => 3 - 2 * x[0], Samples.Select(s => new[] { s }));
        }

        [TestMethod]
        public void Line3NodesAndQuadraticField()
        {
            AssertKronecker(Line3Nodes, (i, x) => QuadraticShapeFunctionLine3.NaturalShapeFunction(i, x[0]));
            AssertReproduces(Line3Nodes, (i, x) => QuadraticShapeFunctionLine3.NaturalShapeFunction(i, x[0]),
                x => 1 + 2 * x[0] - 5 * x[0] * x[0], Samples.Select(s => new[] { s }));
        }

        [TestMethod]
        public void Quad4NodesAndBilinearField()
        {
            Func<int, double[], double> n = (i, x) => LinearShapeFunctionQuad4.NaturalShapeFunction(i, x[0], x[1]);
            AssertKronecker(Quad4Nodes, n);
            AssertReproduces(Quad4Nodes, n, x => 2 + 3 * x[0] - x[1] + 4 * x[0] * x[1], Grid2());
        }

        [TestMethod]
        public void Quad4ReducedDerivativeOverloadsAreEqual()
        {
            foreach (var p in Grid2())
                for (int i = 1; i <= 4; i++)
                {
                    Assert.AreEqual(LinearShapeFunctionQuad4.DNdCsi(i, p[0], p[1]), LinearShapeFunctionQuad4.DNdCsi(i, p[1]), 0.0);
                    Assert.AreEqual(LinearShapeFunctionQuad4.DNdEta(i, p[0], p[1]), LinearShapeFunctionQuad4.DNdEta(i, p[0]), 0.0);
                }
        }

        [TestMethod]
        public void Quad8NodesAndSerendipityField()
        {
            // serendipity space: 1, x, y, x², xy, y², x²y, xy²
            Func<int, double[], double> n = (i, x) => QuadraticShapeFunctionQuad8.NaturalShapeFunction(i, x[0], x[1]);
            AssertKronecker(Quad8Nodes, n);
            AssertReproduces(Quad8Nodes, n, x => 1 - x[0] + 2 * x[1] + 3 * x[0] * x[0] - x[0] * x[1] + 0.5 * x[1] * x[1]
                + 0.7 * x[0] * x[0] * x[1] - 1.3 * x[0] * x[1] * x[1], Grid2());
        }

        [TestMethod]
        public void Tri3NodesAndLinearField()
        {
            Func<int, double[], double> n = (i, x) => LinearShapeFunctionsTri3.NaturalShapeFunction(i, x[0], x[1]);
            AssertKronecker(Tri3Nodes, n);
            AssertReproduces(Tri3Nodes, n, x => 5 - 2 * x[0] + 7 * x[1], Triangle());
            for (int i = 1; i <= 3; i++)
            {
                Assert.AreEqual(LinearShapeFunctionsTri3.DNdCsi(i, 0.2, 0.3), LinearShapeFunctionsTri3.DNdCsi(i), 0.0);
                Assert.AreEqual(LinearShapeFunctionsTri3.DNdEta(i, 0.2, 0.3), LinearShapeFunctionsTri3.DNdEta(i), 0.0);
            }
        }

        [TestMethod]
        public void Tri3LocalShapeFunctionsReproduceTheCoordinates()
        {
            double x1 = -2, y1 = 1, x2 = 6, y2 = -1, x3 = 1, y3 = 5;
            foreach (var p in Triangle())
            {
                // point inside the triangle from its natural coordinates
                double x = p[0] * x1 + p[1] * x2 + (1 - p[0] - p[1]) * x3, y = p[0] * y1 + p[1] * y2 + (1 - p[0] - p[1]) * y3;
                LinearShapeFunctionsTri3.LocalShapeFunction(x, y, x1, y1, x2, y2, x3, y3, out double n1, out double n2, out double n3);
                Assert.AreEqual(p[0], n1, 1e-12);
                Assert.AreEqual(p[1], n2, 1e-12);
                Assert.AreEqual(x, n1 * x1 + n2 * x2 + n3 * x3, 1e-12);
                Assert.AreEqual(y, n1 * y1 + n2 * y2 + n3 * y3, 1e-12);
            }
            // outside the triangle one function is negative
            LinearShapeFunctionsTri3.LocalShapeFunction(20, 20, x1, y1, x2, y2, x3, y3, out double o1, out double o2, out double o3);
            Assert.IsTrue(Math.Min(o1, Math.Min(o2, o3)) < 0);
            Assert.AreEqual(1.0, o1 + o2 + o3, 1e-12);
        }

        [TestMethod]
        public void Tri6NodesAndCompleteQuadraticField()
        {
            Func<int, double[], double> n = (i, x) => QuadraticShapeFunctionsTri6.NaturalShapeFunction(i, x[0], x[1]);
            AssertKronecker(Tri6Nodes, n);
            AssertReproduces(Tri6Nodes, n, x => 2 + x[0] - 3 * x[1] + 4 * x[0] * x[0] - 2 * x[0] * x[1] + 1.5 * x[1] * x[1], Triangle());
        }

        [TestMethod]
        public void Hexa8NodesAndTrilinearField()
        {
            Func<int, double[], double> n = (i, x) => LinearShapeFunctionHexaedron8.NaturalShapeFunction(i, x[0], x[1], x[2]);
            AssertKronecker(Hexa8Nodes, n);
            AssertReproduces(Hexa8Nodes, n, x => 1 + x[0] - 2 * x[1] + 3 * x[2] + x[0] * x[1] - x[1] * x[2] + 2 * x[0] * x[2] + 0.5 * x[0] * x[1] * x[2],
                from a in Samples from b in Samples from c in Samples select new[] { a, b, c });
        }

        [TestMethod]
        public void Pentahedron6ReproducesLinearFieldTimesZeta()
        {
            double[][] nodes = { new[] { 1.0, 0, -1 }, new[] { 0.0, 1, -1 }, new[] { 0.0, 0, -1 }, new[] { 1.0, 0, 1 }, new[] { 0.0, 1, 1 }, new[] { 0.0, 0, 1 } };
            Func<int, double[], double> n = (i, x) => LinearShapeFunctionPentahedron6.NaturalShapeFunction(i, x[0], x[1], x[2]);
            AssertReproduces(nodes, n, x => 1 + 2 * x[0] - x[1] + 3 * x[2] + x[0] * x[2] - 2 * x[1] * x[2],
                from p in Triangle() from z in Samples select new[] { p[0], p[1], z });
        }

        [TestMethod]
        public void DerivativesSumToZero()
        {
            foreach (var p in Grid2())
            {
                Assert.AreEqual(0, Enumerable.Range(1, 4).Sum(i => LinearShapeFunctionQuad4.DNdCsi(i, p[0], p[1])), 1e-14);
                Assert.AreEqual(0, Enumerable.Range(1, 8).Sum(i => QuadraticShapeFunctionQuad8.DNdEta(i, p[0], p[1])), 1e-14);
                Assert.AreEqual(0, Enumerable.Range(1, 6).Sum(i => QuadraticShapeFunctionsTri6.DNdCsi(i, p[0], p[1])), 1e-14);
                Assert.AreEqual(0, Enumerable.Range(1, 8).Sum(i => LinearShapeFunctionHexaedron8.DNdZeta(i, p[0], p[1], 0.3)), 1e-14);
                Assert.AreEqual(0, Enumerable.Range(1, 3).Sum(i => QuadraticShapeFunctionLine3.DNdCsi(i, p[0])), 1e-14);
            }
        }

        [TestMethod]
        public void InvalidIndexThrows()
        {
            Assert.ThrowsException<ArgumentException>(() => LinearShapeFunctionsLine2.NaturalShapeFunction(3, 0));
            Assert.ThrowsException<ArgumentException>(() => QuadraticShapeFunctionLine3.DNdCsi(0, 0));
            Assert.ThrowsException<ArgumentException>(() => LinearShapeFunctionQuad4.NaturalShapeFunction(5, 0, 0));
            Assert.ThrowsException<ArgumentException>(() => QuadraticShapeFunctionQuad8.DNdCsi(9, 0, 0));
            Assert.ThrowsException<ArgumentException>(() => LinearShapeFunctionsTri3.LocalShapeFunction(4, 0, 0, 0, 0, 1, 0, 0, 1));
            Assert.ThrowsException<ArgumentException>(() => QuadraticShapeFunctionsTri6.DNdEta(7, 0, 0));
            Assert.ThrowsException<ArgumentException>(() => LinearShapeFunctionPentahedron6.DNdZeta(0, 0, 0, 0));
        }
    }

    [TestClass]
    public class UnitsConvertControlTest
    {
        [TestMethod]
        public void LengthDefinitions()
        {
            Assert.AreEqual(1000.0, UnitsConvert.Convert(1, LengthUnits.m, LengthUnits.mm, 1), 1e-12);
            Assert.AreEqual(25.4, UnitsConvert.Convert(1, LengthUnits.inch, LengthUnits.mm, 1), 1e-12);
            Assert.AreEqual(12.0, UnitsConvert.Convert(1, LengthUnits.ft, LengthUnits.inch, 1), 1e-12);
            Assert.AreEqual(0.1, UnitsConvert.Convert(1, LengthUnits.mm, LengthUnits.cm, 1), 1e-15);
        }

        [TestMethod]
        public void LengthExponents()
        {
            Assert.AreEqual(1e6, UnitsConvert.Convert(1, LengthUnits.m, LengthUnits.mm, 2), 1e-6);
            Assert.AreEqual(1e9, UnitsConvert.Convert(1, LengthUnits.m, LengthUnits.mm, 3), 1e-3);
            Assert.AreEqual(1e12, UnitsConvert.Convert(1, LengthUnits.m, LengthUnits.mm, 4), 1);
            // stiffness per length: 1 /mm = 1000 /m
            Assert.AreEqual(1000.0, UnitsConvert.Convert(1, LengthUnits.mm, LengthUnits.m, -1), 1e-9);
            Assert.AreEqual(5.0, UnitsConvert.Convert(5, LengthUnits.ft, LengthUnits.cm, 0), 0.0);
        }

        [TestMethod]
        public void ForceDefinitions()
        {
            Assert.AreEqual(1000.0, UnitsConvert.Convert(1, ForceUnits.kN, ForceUnits.N, 1), 1e-12);
            Assert.AreEqual(100.0, UnitsConvert.Convert(1, ForceUnits.kN, ForceUnits.daN, 1), 1e-12);
            // exact definitions: 1 lbf = 0.45359237 kg × 9.80665 m/s² = 4.4482216152605 N, 1 kip = 1000 lbf
            Assert.AreEqual(4.4482216152605, UnitsConvert.Convert(1, ForceUnits.lbf, ForceUnits.N, 1), 1e-12);
            Assert.AreEqual(4448.2216152605, UnitsConvert.Convert(1, ForceUnits.kip, ForceUnits.N, 1), 1e-9);
            Assert.AreEqual(1000.0, UnitsConvert.Convert(1, ForceUnits.kip, ForceUnits.lbf, 1), 1e-10);
        }

        [TestMethod]
        public void MomentAndStressFromForceAndLength()
        {
            // 1 kNm = 1e6 Nmm; 1 kN/m² = 1e-3 N/mm²; 1 kN/m = 1 N/mm
            Assert.AreEqual(1e6, UnitsConvert.Convert(1, ForceUnits.kN, ForceUnits.N, 1, LengthUnits.m, LengthUnits.mm, 1), 1e-6);
            Assert.AreEqual(1e-3, UnitsConvert.Convert(1, ForceUnits.kN, ForceUnits.N, 1, LengthUnits.m, LengthUnits.mm, -2), 1e-15);
            Assert.AreEqual(1.0, UnitsConvert.Convert(1, ForceUnits.kN, ForceUnits.N, 1, LengthUnits.m, LengthUnits.mm, -1), 1e-15);
            Assert.AreEqual(1e6, UnitsConvert.ConvertToDefaultUnits(1, ForceUnits.kN, 1, LengthUnits.m, 1), 1e-6);
            Assert.AreEqual(1.0, UnitsConvert.ConvertFromDefaultUnits(1e6, ForceUnits.kN, 1, LengthUnits.m, 1), 1e-12);
        }

        [TestMethod]
        public void PressureDefinitions()
        {
            Assert.AreEqual(1e6, UnitsConvert.Convert(1, PressureUnits.MPa, PressureUnits.Pa, 1), 1e-6);
            Assert.AreEqual(1000.0, UnitsConvert.Convert(1, PressureUnits.MPa, PressureUnits.kPa, 1), 1e-9);
            // 1 psi = 1 lbf/in² = 6894.757293168 Pa, 1 ksi = 1000 psi
            Assert.AreEqual(6894.757293168, UnitsConvert.Convert(1, PressureUnits.psi, PressureUnits.Pa, 1), 1e-8);
            Assert.AreEqual(6.894757293168, UnitsConvert.Convert(1, PressureUnits.ksi, PressureUnits.MPa, 1), 1e-11);
            Assert.AreEqual(1000.0, UnitsConvert.Convert(1, PressureUnits.ksi, PressureUnits.psi, 1), 1e-10);
            // psi coherent with lbf and inch
            Assert.AreEqual(UnitsConvert.Convert(1, ForceUnits.lbf, ForceUnits.N, 1, LengthUnits.inch, LengthUnits.mm, -2),
                UnitsConvert.Convert(1, PressureUnits.psi, PressureUnits.MPa, 1), 1e-15);
            Assert.AreEqual(1.0, UnitsConvert.ConvertToDefaultUnits(1000, PressureUnits.kPa, 1), 1e-12);
        }

        [TestMethod]
        public void MassAndDensity()
        {
            Assert.AreEqual(1000.0, UnitsConvert.Convert(1, MassUnits.ton, MassUnits.kg, 1), 1e-12);
            // 1 lb = 0.45359237 kg
            Assert.AreEqual(0.45359237, UnitsConvert.Convert(1, MassUnits.lb, MassUnits.kg, 1), 1e-15);
            // steel 7850 kg/m³ = 7.85e-9 ton/mm³ (default units)
            Assert.AreEqual(7.85e-9, UnitsConvert.Convert(7850, MassUnits.kg, MassUnits.ton, 1, LengthUnits.m, LengthUnits.mm, -3), 1e-20);
            Assert.AreEqual(7.85e-9, UnitsConvert.ConvertToDefaultUnits(7850, MassUnits.kg, 1, LengthUnits.m, -3), 1e-20);
            Assert.AreEqual(7850, UnitsConvert.ConvertFromDefaultUnits(7.85e-9, MassUnits.kg, 1, LengthUnits.m, -3), 1e-6);
        }

        [TestMethod]
        public void RoundTripOfEveryPairAndExponent()
        {
            foreach (int e in new[] { -3, -2, -1, 0, 1, 2, 3 })
            {
                foreach (ForceUnits a in Enum.GetValues(typeof(ForceUnits)))
                    foreach (ForceUnits b in Enum.GetValues(typeof(ForceUnits)))
                        Assert.AreEqual(3.7, UnitsConvert.Convert(UnitsConvert.Convert(3.7, a, b, e), b, a, e), 3.7e-12, $"{a} {b} {e}");
                foreach (LengthUnits a in Enum.GetValues(typeof(LengthUnits)))
                    foreach (LengthUnits b in Enum.GetValues(typeof(LengthUnits)))
                        Assert.AreEqual(3.7, UnitsConvert.Convert(UnitsConvert.Convert(3.7, a, b, e), b, a, e), 3.7e-12, $"{a} {b} {e}");
                foreach (MassUnits a in Enum.GetValues(typeof(MassUnits)))
                    foreach (MassUnits b in Enum.GetValues(typeof(MassUnits)))
                        Assert.AreEqual(3.7, UnitsConvert.Convert(UnitsConvert.Convert(3.7, a, b, e), b, a, e), 3.7e-12, $"{a} {b} {e}");
                foreach (PressureUnits a in Enum.GetValues(typeof(PressureUnits)))
                    foreach (PressureUnits b in Enum.GetValues(typeof(PressureUnits)))
                        Assert.AreEqual(3.7, UnitsConvert.Convert(UnitsConvert.Convert(3.7, a, b, e), b, a, e), 3.7e-12, $"{a} {b} {e}");
            }
        }

        [TestMethod]
        public void ConversionIsTransitive()
        {
            // m -> inch -> mm equals m -> mm
            double viaInch = UnitsConvert.Convert(UnitsConvert.Convert(2.5, LengthUnits.m, LengthUnits.inch, 2), LengthUnits.inch, LengthUnits.mm, 2);
            Assert.AreEqual(UnitsConvert.Convert(2.5, LengthUnits.m, LengthUnits.mm, 2), viaInch, 1e-6);
            double viaKip = UnitsConvert.Convert(UnitsConvert.Convert(10, ForceUnits.kN, ForceUnits.kip, 1), ForceUnits.kip, ForceUnits.daN, 1);
            Assert.AreEqual(1000.0, viaKip, 1e-9);
        }

        [TestMethod]
        public void TemperatureAbsoluteAndDifference()
        {
            Assert.AreEqual(212.0, UnitsConvert.Convert(100.0, TemperatureUnits.C, TemperatureUnits.F), 1e-12);
            Assert.AreEqual(32.0, UnitsConvert.Convert(0.0, TemperatureUnits.C, TemperatureUnits.F), 1e-12);
            Assert.AreEqual(0.0, UnitsConvert.Convert(32.0, TemperatureUnits.F, TemperatureUnits.C), 1e-12);
            // a thermal load ΔT = 30 °C is 54 °F, not 86 °F
            Assert.AreEqual(54.0, UnitsConvert.ConvertDifference(30, TemperatureUnits.C, TemperatureUnits.F), 1e-12);
            Assert.AreEqual(37.5, UnitsConvert.Convert(UnitsConvert.Convert(37.5, TemperatureUnits.C, TemperatureUnits.F), TemperatureUnits.F, TemperatureUnits.C), 1e-12);
        }

        [TestMethod]
        public void DefaultUnitsAreNmmTonMPa()
        {
            Assert.AreEqual(ForceUnits.N, DefaultForceUnits);
            Assert.AreEqual(LengthUnits.mm, DefaultLengthUnits);
            Assert.AreEqual(MassUnits.ton, DefaultMassUnits);
            Assert.AreEqual(PressureUnits.MPa, DefaultPressureUnits);
            // consistency of the default system (ton, mm, s -> N): the weight of 1 ton is 9806.65 N = 9.80665 kN
            Assert.AreEqual(9.80665, UnitsConvert.ConvertFromDefaultUnits(1 * Constants.Constants.GRAVITYACCELERATION, ForceUnits.kN, 1), 1e-12);
        }
    }

    [TestClass]
    public class MathsControlTest
    {
        [TestMethod]
        public void DegreesAndRadiansOfEveryType()
        {
            Assert.AreEqual(Math.PI, 180.0.ToRadians(), 1e-15);
            Assert.AreEqual(Math.PI, 180f.ToRadians(), 1e-15);
            Assert.AreEqual(Math.PI, 180.ToRadians(), 1e-15);
            Assert.AreEqual(Math.PI, 180L.ToRadians(), 1e-15);
            Assert.AreEqual(Math.PI, ((short)180).ToRadians(), 1e-15);
            Assert.AreEqual((double)Math.PI, (double)180m.ToRadians(), 1e-12);
            Assert.AreEqual(180.0, Math.PI.ToDegrees(), 1e-12);
            Assert.AreEqual(57.29577951308232, 1.ToDegrees(), 1e-12);
            Assert.AreEqual(0.0, 0L.ToDegrees(), 0.0);
            Assert.AreEqual(37.25, 37.25.ToRadians().ToDegrees(), 1e-12);
        }

        [TestMethod]
        public void RoundToMultipleOfIntegers()
        {
            Assert.AreEqual(0, 0.RoundToMultiple(5));
            Assert.AreEqual(5, 1.RoundToMultiple(5));
            Assert.AreEqual(-10, (-12).RoundToMultiple(5));
            Assert.AreEqual(250, 201.RoundToMultiple(50));
        }

        [TestMethod]
        public void RoundToMultipleOfDoublesKeepsSpecialValues()
        {
            Assert.IsTrue(double.IsNaN(double.NaN.RoundToMultiple(0.5)));
            Assert.AreEqual(double.PositiveInfinity, double.PositiveInfinity.RoundToMultiple(0.5));
            Assert.AreEqual(0.35, 0.3000001.RoundToMultiple(0.05));
            Assert.AreEqual(1.2, 1.2000000000000002.RoundToMultiple(0.1));
            Assert.AreEqual(12.35, 12.341.RoundToMultiple(0.05, 2));
        }

        [TestMethod]
        public void LinearInterpolationOnTheNodesAndOnDescendingSegments()
        {
            double[] xi = { 0, 2, 5, 9 };
            double[] yi = { 10, 4, 4, -8 };
            for (int i = 0; i < xi.Length; i++)
                Assert.AreEqual(yi[i], Interpolation.GetLinearInterpolation(xi, yi, xi[i]), 1e-12);
            Assert.AreEqual(7.0, Interpolation.GetLinearInterpolation(xi, yi, 1), 1e-12);
            Assert.AreEqual(4.0, Interpolation.GetLinearInterpolation(xi, yi, 3.3), 1e-12);
            Assert.AreEqual(-5.0, Interpolation.GetLinearInterpolation(xi, yi, 8), 1e-12);
            Assert.AreEqual(-2.0, Interpolation.GetLinearInterpolation(5, 9, 4, -8, 7), 1e-12);
            // extrapolation of the two-points formula is linear
            Assert.AreEqual(-14.0, Interpolation.GetLinearInterpolation(5, 9, 4, -8, 11), 1e-12);
        }

        [TestMethod]
        public void QuadraticInterpolationIsExactForParabolas()
        {
            Func<double, double> f = x => 3 * x * x - 2 * x + 1;
            foreach (double x in new[] { -3.0, 0.5, 1.7, 4 })
                Assert.AreEqual(f(x), Interpolation.GetQuadraticInterpolation(-1, 1, 2.5, f(-1), f(1), f(2.5), x), 1e-10);
            Assert.AreEqual(9.0, Interpolation.GetQuadraticInterpolation(0, 1, 3, 0, 1, 9, 3), 1e-12);
        }

        [TestMethod]
        public void NullArraysThrow()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Interpolation.GetLinearInterpolation(null, new double[] { 1 }, 0));
            Assert.ThrowsException<ArgumentNullException>(() => Interpolation.GetLinearInterpolation(new double[] { 1 }, null, 0));
            Assert.AreEqual(7.0, Interpolation.GetLinearInterpolation(new double[] { 1 }, new double[] { 7 }, 3), 0.0);
        }

        [TestMethod]
        public void ArithmeticMeanIsIncremental()
        {
            Assert.AreEqual(2.5, Averages.ArithmeticMean(new[] { 1.0, 2, 3, 4 }), 1e-15);
            Assert.AreEqual(2.5, Averages.ArithmeticMean(new[] { 1, 2, 3, 4 }), 1e-15);
            Assert.AreEqual(0.0, Averages.ArithmeticMean(Enumerable.Empty<double>()), 0.0);
            // the running mean does not overflow where the sum would
            Assert.AreEqual(1e308, Averages.ArithmeticMean(new[] { 1e308, 1e308, 1e308 }), 1e294);
            Assert.AreEqual(int.MaxValue, Averages.ArithmeticMean(new[] { int.MaxValue, int.MaxValue }), 1e-6);
        }

        [TestMethod]
        public void AbsoluteAndRelativeErrors()
        {
            Assert.AreEqual(1.0, Error.CalcAbsoluteError(11, 10), 1e-15);
            Assert.AreEqual(0.1, Error.CalcRelativeError(11, 10), 1e-15);
            Assert.AreEqual(-0.1, Error.CalcRelativeError(9, 10), 1e-15);
            Assert.AreEqual(0.0, Error.CalcRelativeError(0, 0), 0.0);
            Assert.AreEqual(5.0, Error.CalcRelativeError(5, 0), 0.0);
            // relative to the mean: |9 - 10| / 10
            Assert.AreEqual(0.1, Error.TwoValuesRelativeError(9, 11), 1e-15);
            Assert.AreEqual(3.0, Error.TwoValuesRelativeError(0, 3), 0.0);
            Assert.IsTrue(Error.AreEqualsDouble(1, 1 + 1e-15));
            Assert.IsFalse(Error.AreEqualsDouble(1, 1.001));
            Assert.IsTrue(Error.AreEqualsDouble(1, 1.001, 1e-2));
        }

        [TestMethod]
        public void ErrorPropagationOfSumsAndSquares()
        {
            Assert.AreEqual(0.5, ErrorPropagation.SumTolerance(0.3, 0.4), 1e-15);
            Assert.AreEqual(0.25, ErrorPropagation.SumSquareTolerance(0.3, 0.4), 1e-15);
            Assert.AreEqual(Math.Sqrt(1.4142) * 0.1, ErrorPropagation.DefaultProductTolerance(0.1), 1e-15);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ErrorPropagation.SumTolerance(double.NaN, 0));
            // a product with one exact term: t = |a| tb
            Assert.AreEqual(0.6, ErrorPropagation.ProductTolerance(3, 5, 0, 0.2, 1e-30), 1e-15);
            Assert.AreEqual(0.05, ErrorPropagation.DivisionTolerance(10, 2, 0.1, 0, 1e-30), 1e-15);
        }

        [TestMethod]
        public void ConstantsAreConsistent()
        {
            Assert.AreEqual(9806.65, Constants.Constants.GRAVITYACCELERATION);
            Assert.AreEqual(3600, Constants.Constants.SECONDSINAHOUR);
            Assert.AreEqual(24 * Constants.Constants.SECONDSINAHOUR, Constants.Constants.SECONDSINADAY);
            Assert.AreEqual(31536000, Constants.Constants.SECONDSINAYEAR);
        }
    }

    [TestClass]
    public class ExtensionsControlTest
    {
        private class Base<T> { }
        private class Derived : Base<int> { }
        private class DerivedAgain : Derived { }

        private enum Sample : byte
        {
            [System.ComponentModel.Description("First value")] A = 1,
            B = 7
        }

        public class Node
        {
            public int Value;
            public string Name;
            public List<int> Items = new List<int>();
            public double[] Array;
            public Node Next;
            public Action Callback;
        }

        [TestMethod]
        public void ScrambledEqualsCountsRepetitions()
        {
            Assert.IsTrue(new[] { 1, 2, 2, 3 }.ScrambledEquals(new[] { 3, 2, 1, 2 }));
            Assert.IsFalse(new[] { 1, 2, 2 }.ScrambledEquals(new[] { 1, 1, 2 }));
            Assert.IsFalse(new[] { 1, 2 }.ScrambledEquals(new[] { 1, 2, 2 }));
            Assert.IsFalse(new[] { 1, 2, 2 }.ScrambledEquals(new[] { 1, 2 }));
            Assert.IsTrue(new int[0].ScrambledEquals(new int[0]));
            Assert.IsTrue(new[] { "a", "B" }.ScrambledEquals(new[] { "b", "A" }, StringComparer.OrdinalIgnoreCase));
            Assert.IsFalse(new[] { "a", "B" }.ScrambledEquals(new[] { "b", "A" }));
        }

        [TestMethod]
        public void SequenceHashDependsOnOrderScrambledHashDoesNot()
        {
            Assert.AreNotEqual(new[] { 1, 2, 3 }.GetHashCodeSequence(), new[] { 3, 2, 1 }.GetHashCodeSequence());
            Assert.AreEqual(new[] { 1, 2, 3 }.GetHashCodeScrambled(), new[] { 3, 1, 2 }.GetHashCodeScrambled());
            Assert.AreEqual(new List<int> { 4, 5 }.GetHashCodeSequence(), new[] { 4, 5 }.GetHashCodeSequence());
        }

        [TestMethod]
        public async Task ParallelForEachReturnsEveryResult()
        {
            var result = await Enumerable.Range(0, 100).ParallelForEachAsync(i => Task.FromResult(i * i), 7);
            Assert.IsTrue(result.ScrambledEquals(Enumerable.Range(0, 100).Select(i => i * i)));
            var withInput = await Enumerable.Range(0, 10).ParallelForEachAsync((i, k) => Task.FromResult(i + k), 100, 3);
            Assert.IsTrue(withInput.ScrambledEquals(Enumerable.Range(100, 10)));
        }

        [TestMethod]
        public void SplitIntoExactChunks()
        {
            var chunks = Enumerable.Range(0, 6).ToList().Split(2);
            Assert.AreEqual(3, chunks.Count);
            CollectionAssert.AreEqual(new[] { 4, 5 }, chunks[2]);
            Assert.AreEqual(1, new[] { 1, 2 }.Split(10).Count);
            Assert.AreEqual(0, new List<int>().Split(1).Count);
        }

        [TestMethod]
        public void FirstLetterToUpperCase()
        {
            Assert.AreEqual("Trave", "trave".FirstLetterToUpperCase());
            Assert.AreEqual("À", "à".FirstLetterToUpperCase());
            Assert.AreEqual("1a", "1a".FirstLetterToUpperCase());
            Assert.ThrowsException<ArgumentException>(() => "".FirstLetterToUpperCase());
            Assert.AreEqual(string.Empty, ((string)null).FirstLetterToUpperCaseOrConvertNullToEmptyString());
            Assert.AreEqual("X", "x".FirstLetterToUpperCaseOrConvertNullToEmptyString());
        }

        [TestMethod]
        public void EnumDescriptionsAndLists()
        {
            Assert.AreEqual("First value", Sample.A.GetDescription());
            Assert.AreEqual("B", Sample.B.GetDescription());
            Assert.AreEqual("In", LengthUnits.inch.GetDescription());
            var all = EnumExtension.GetAllValuesAndDescriptions(typeof(Sample)).ToList();
            CollectionAssert.AreEqual(new object[] { "First value", "B" }, all.Select(a => a.Description).ToArray());
            Assert.ThrowsException<ArgumentException>(() => EnumExtension.GetAllValuesAndDescriptions(typeof(int)));
            // byte-based enum
            CollectionAssert.AreEqual(new[] { new KeyValuePair<string, int>("A", 1), new KeyValuePair<string, int>("B", 7) }, EnumHelper.GetEnumList<Sample>().ToArray());
        }

        [TestMethod]
        public void RawGenericSubclass()
        {
            Assert.IsTrue(typeof(Base<>).IsSubclassOfRawGeneric(typeof(DerivedAgain)));
            Assert.IsTrue(typeof(Base<>).IsSubclassOfRawGeneric(typeof(Base<string>)));
            Assert.IsFalse(typeof(List<>).IsSubclassOfRawGeneric(typeof(DerivedAgain)));
            Assert.IsFalse(typeof(Base<>).IsSubclassOfRawGeneric(null));
        }

        [TestMethod]
        public void DeepCopyIsIndependentAndKeepsCycles()
        {
            var original = new Node { Value = 3, Name = "a", Items = { 1, 2 }, Array = new[] { 1.5, 2.5 }, Callback = () => { } };
            original.Next = original;
            var copy = original.DeepCopyByExpressionTree();
            Assert.AreNotSame(original, copy);
            Assert.AreEqual(3, copy.Value);
            Assert.AreEqual("a", copy.Name);
            Assert.AreNotSame(original.Items, copy.Items);
            Assert.AreNotSame(original.Array, copy.Array);
            CollectionAssert.AreEqual(original.Items, copy.Items);
            Assert.AreSame(copy, copy.Next, "the cycle points to the copy");
            Assert.IsNull(copy.Callback, "delegates are not copied");
            copy.Items.Add(9);
            copy.Array[0] = -1;
            Assert.AreEqual(2, original.Items.Count);
            Assert.AreEqual(1.5, original.Array[0]);
            Assert.IsNull(((Node)null).DeepCopyByExpressionTree());
        }
    }
}
