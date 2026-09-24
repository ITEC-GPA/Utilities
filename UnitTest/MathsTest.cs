using System;
using System.Collections.Generic;
using System.Linq;
using GPC.Utilities.Extensions;
using GPC.Utilities.Maths;
using GPC.Utilities.Units;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GPC.Utilities.UnitTest
{
    [TestClass]
    public class UnitsConvertTest
    {
        [TestMethod]
        public void TemperatureSameUnitsIsIdentity()
        {
            Assert.AreEqual(20.0, UnitsConvert.Convert(20.0, UnitsConvert.TemperatureUnits.C, UnitsConvert.TemperatureUnits.C), 1e-12);
            Assert.AreEqual(68.0, UnitsConvert.Convert(68.0, UnitsConvert.TemperatureUnits.F, UnitsConvert.TemperatureUnits.F), 1e-12);
        }

        [TestMethod]
        public void TemperatureCelsiusFahrenheit()
        {
            Assert.AreEqual(68.0, UnitsConvert.Convert(20.0, UnitsConvert.TemperatureUnits.C, UnitsConvert.TemperatureUnits.F), 1e-12);
            Assert.AreEqual(20.0, UnitsConvert.Convert(68.0, UnitsConvert.TemperatureUnits.F, UnitsConvert.TemperatureUnits.C), 1e-12);
            Assert.AreEqual(-40.0, UnitsConvert.Convert(-40.0, UnitsConvert.TemperatureUnits.C, UnitsConvert.TemperatureUnits.F), 1e-12);
        }

        [TestMethod]
        public void TemperatureDifference()
        {
            Assert.AreEqual(18.0, UnitsConvert.ConvertDifference(10.0, UnitsConvert.TemperatureUnits.C, UnitsConvert.TemperatureUnits.F), 1e-12);
            Assert.AreEqual(10.0, UnitsConvert.ConvertDifference(18.0, UnitsConvert.TemperatureUnits.F, UnitsConvert.TemperatureUnits.C), 1e-12);
            Assert.AreEqual(10.0, UnitsConvert.ConvertDifference(10.0, UnitsConvert.TemperatureUnits.C, UnitsConvert.TemperatureUnits.C), 1e-12);
        }
    }

    [TestClass]
    public class ErrorPropagationTest
    {
        [TestMethod]
        public void ProductOfThreeTerms()
        {
            // t^2 * [(b c)^2 + (a c)^2 + (a b)^2] = 0.01 * (36 + 9 + 4)
            Assert.AreEqual(0.7, ErrorPropagation.ProductTolerance(1, 2, 3, 0.1, 0.1, 0.1), 1e-12);
            Assert.AreEqual(0.49, ErrorPropagation.ProductSquareTolerance(1, 2, 3, 0.1, 0.1, 0.1), 1e-12);
        }

        [TestMethod]
        public void ProductOfTwoTermsUnchanged()
        {
            Assert.AreEqual(Math.Sqrt(0.01 * 4 + 0.04 * 1), ErrorPropagation.ProductTolerance(1, 2, 0.1, 0.2), 1e-12);
            // The minimum tolerance is still sqrt(ta * tb), the geometry relies on it
            Assert.AreEqual(1e-4, ErrorPropagation.ProductTolerance(0, 0, 1e-4, 1e-4), 1e-16);
            // Explicit minimum tolerance is now honoured
            Assert.AreEqual(0.5, ErrorPropagation.ProductTolerance(0, 0, 1e-4, 1e-4, 0.25), 1e-12);
        }

        [TestMethod]
        public void ZeroToleranceIsAllowed()
        {
            Assert.AreEqual(0.001, ErrorPropagation.SumTolerance(0.001, 0), 1e-15);
            Assert.AreEqual(0.0, ErrorPropagation.SumTolerance(0, 0), 0.0);
            Assert.AreEqual(0.0, ErrorPropagation.ProductTolerance(3, 4, 0, 0), 0.0);
        }

        [TestMethod]
        public void NegativeToleranceThrows()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ErrorPropagation.SumTolerance(-1e-4, 1e-4));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ErrorPropagation.ProductTolerance(1, 1, 1e-4, -1e-4));
        }

        [TestMethod]
        public void Division()
        {
            // (ta/b)^2 + (a tb / b^2)^2
            Assert.AreEqual(Math.Sqrt(Math.Pow(0.1 / 2, 2) + Math.Pow(3 * 0.2 / 4, 2)), ErrorPropagation.DivisionTolerance(3, 2, 0.1, 0.2), 1e-12);
            Assert.ThrowsException<ArgumentException>(() => ErrorPropagation.DivisionTolerance(3, 0, 0.1, 0.2));
        }
    }

    [TestClass]
    public class MathExtensionTest
    {
        [DataTestMethod]
        [DataRow(0.23, 0.05, 0.25)]
        [DataRow(0.3, 0.1, 0.3)]
        [DataRow(0.30000000000000004, 0.1, 0.3)]
        [DataRow(0.31, 0.1, 0.4)]
        [DataRow(-0.23, 0.05, -0.2)]
        [DataRow(7.0, 5.0, 10.0)]
        [DataRow(-7.0, 5.0, -5.0)]
        [DataRow(10.0, 5.0, 10.0)]
        [DataRow(1234.5, 100.0, 1300.0)]
        [DataRow(0.23, -0.05, 0.25)]
        public void RoundDoubleToMultiple(double value, double multiple, double expected)
        {
            Assert.AreEqual(expected, value.RoundToMultiple(multiple));
        }

        [TestMethod]
        public void RoundDoubleToMultipleWithDigits()
        {
            Assert.AreEqual(1.0, 0.8.RoundToMultiple(0.25, 0));
            Assert.AreEqual(5.0, 5.0.RoundToMultiple(0));
        }

        [DataTestMethod]
        [DataRow(7, 5, 10)]
        [DataRow(-7, 5, -5)]
        [DataRow(10, 5, 10)]
        [DataRow(7, -5, 10)]
        [DataRow(7, 0, 7)]
        public void RoundIntToMultiple(int value, int multiple, int expected)
        {
            Assert.AreEqual(expected, value.RoundToMultiple(multiple));
        }
    }

    [TestClass]
    public class SplitTest
    {
        [TestMethod]
        public void SplitList()
        {
            var chunks = Enumerable.Range(0, 7).ToList().Split(3);
            CollectionAssert.AreEqual(new[] { 3, 3, 1 }, chunks.Select(c => c.Count).ToArray());
            CollectionAssert.AreEqual(new[] { 6 }, chunks[2]);
        }

        [TestMethod]
        public void SplitArray()
        {
            var chunks = Enumerable.Range(0, 7).ToArray().Split(3);
            CollectionAssert.AreEqual(new[] { 3, 3, 1 }, chunks.Select(c => c.Length).ToArray());
            CollectionAssert.AreEqual(new[] { 3, 4, 5 }, chunks[1]);
            Assert.AreEqual(0, new int[0].Split(3).Count);
        }

        [TestMethod]
        public void SplitWithInvalidSizeThrows()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new List<int> { 1, 2 }.Split(0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new[] { 1, 2 }.Split(0));
        }
    }

    [TestClass]
    public class InterpolationTest
    {
        [TestMethod]
        public void LinearInterpolation()
        {
            double[] xi = { 0, 10, 20 };
            double[] yi = { 0, 100, 50 };
            Assert.AreEqual(50.0, Interpolation.GetLinearInterpolation(xi, yi, 5), 1e-12);
            Assert.AreEqual(75.0, Interpolation.GetLinearInterpolation(xi, yi, 15), 1e-12);
            Assert.AreEqual(0.0, Interpolation.GetLinearInterpolation(xi, yi, -5), 1e-12);
            Assert.AreEqual(50.0, Interpolation.GetLinearInterpolation(xi, yi, 25), 1e-12);
        }

        [TestMethod]
        public void CoincidentAbscissae()
        {
            Assert.AreEqual(3.0, Interpolation.GetLinearInterpolation(1, 1, 3, 7, 1));
            Assert.AreEqual(100.0, Interpolation.GetLinearInterpolation(new double[] { 0, 10, 10, 20 }, new double[] { 0, 100, 200, 300 }, 10), 1e-12);
        }

        [TestMethod]
        public void InvalidInputs()
        {
            Assert.ThrowsException<ArgumentException>(() => Interpolation.GetLinearInterpolation(new double[0], new double[0], 1));
            Assert.ThrowsException<ArgumentException>(() => Interpolation.GetLinearInterpolation(new double[] { 0, 1 }, new double[] { 0 }, 1));
        }
    }

    [TestClass]
    public class EnumerableExtensionTest
    {
        [TestMethod]
        public void HashCodeWithNullItems()
        {
            var list = new List<string> { "a", null, "b" };
            Assert.AreEqual(list.GetHashCodeSequence(), new List<string> { "a", null, "b" }.GetHashCodeSequence());
            Assert.AreEqual(list.GetHashCodeScrambled(), new List<string> { "b", "a", null }.GetHashCodeScrambled());
        }
    }
}
