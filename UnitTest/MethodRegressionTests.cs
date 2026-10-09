using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GPC.Utilities.Clone;
using GPC.Utilities.Extensions;
using GPC.Utilities.Maths;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GPC.Utilities.UnitTest
{
    [TestClass]
    public class MethodRegressionTests
    {
        [TestMethod]
        public async Task ParallelForEachAsync_ProcessesEveryItemExactlyOnce()
        {
            var visits = new int[12];
            var result = await Enumerable.Range(0, 12).ParallelForEachAsync(async i =>
            {
                await Task.Yield();
                Interlocked.Increment(ref visits[i]);
                return i * i;
            }, maxDoP: 3);
            CollectionAssert.AreEqual(Enumerable.Repeat(1, 12).ToArray(), visits);
            CollectionAssert.AreEqual(new[] { 0, 1, 4, 9, 16, 25, 36, 49, 64, 81, 100, 121 }, result.OrderBy(x => x).ToArray());
        }

        [TestMethod]
        public async Task ParallelForEachAsync_WithSecondInputPassesItToEveryItem()
        {
            var result = await new[] { 1, 3, 7 }.ParallelForEachAsync(
                (int value, int offset) => Task.FromResult(value + offset), 10, maxDoP: 2);
            CollectionAssert.AreEqual(new[] { 11, 13, 17 }, result.OrderBy(x => x).ToArray());
        }

        [TestMethod]
        public async Task ParallelForEachAsync_PropagatesWorkerFailure()
        {
            var error = new InvalidOperationException("worker failure");
            var caught = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
                await new[] { 1 }.ParallelForEachAsync<int, int>(_ => Task.FromException<int>(error), maxDoP: 1));
            Assert.AreSame(error, caught);
        }

        [TestMethod]
        public async Task ParallelForEachAsync_EmptyInputNeverCallsWorker()
        {
            int calls = 0;
            var result = await Array.Empty<int>().ParallelForEachAsync(i =>
            {
                calls++;
                return Task.FromResult(i);
            });
            Assert.AreEqual(0, calls);
            Assert.AreEqual(0, result.Count());
        }

        public sealed class Item { public int Value; }

        [TestMethod]
        public void DeepCopyByExpressionTree_PreservesSharedReferencesInsideRectangularArray()
        {
            var item = new Item { Value = 7 };
            var source = new[,] { { item, null }, { item, new Item { Value = 9 } } };
            var copy = source.DeepCopyByExpressionTree();
            Assert.AreEqual(2, copy.GetLength(0));
            Assert.AreEqual(2, copy.GetLength(1));
            Assert.IsNull(copy[0, 1]);
            Assert.AreSame(copy[0, 0], copy[1, 0]);
            Assert.AreNotSame(item, copy[0, 0]);
            Assert.AreEqual(9, copy[1, 1].Value);
            copy[0, 0].Value = -1;
            Assert.AreEqual(7, item.Value);
        }

        [TestMethod]
        public void ListSplit_ReturnsIndependentContainers()
        {
            var source = new List<int> { 1, 2, 3 };
            var parts = source.Split(2);
            parts[0][0] = 99;
            parts[1].Add(4);
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, source);
        }

        [TestMethod]
        public void ArraySplit_ReturnsIndependentContainers()
        {
            var source = new[] { 1, 2, 3 };
            var parts = source.Split(2);
            parts[0][0] = 99;
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, source);
            CollectionAssert.AreEqual(new[] { 3 }, parts[1]);
        }

        [TestMethod]
        public void AreEqualsDouble_UsesStrictToleranceBoundary()
        {
            Assert.IsTrue(Error.AreEqualsDouble(0, 0.0009, 0.001));
            Assert.IsFalse(Error.AreEqualsDouble(0, 0.001, 0.001));
            Assert.IsFalse(Error.AreEqualsDouble(0, -0.001, 0.001));
        }

        [TestMethod]
        public void TwoValuesRelativeError_UsesAbsoluteErrorBelowExplicitZeroThreshold()
        {
            Assert.AreEqual(0.1, Error.TwoValuesRelativeError(0.1, 0.2, zero: 0.15), 1e-14);
            // Relative deviation of 0.1 from the mean 0.15 is 1/3.
            Assert.AreEqual(1.0 / 3, Error.TwoValuesRelativeError(0.1, 0.2, zero: 0.01), 1e-14);
        }

        [TestMethod]
        public void DivisionSquareTolerance_HonoursExplicitSquaredFloor()
        {
            Assert.AreEqual(0.25, ErrorPropagation.DivisionSquareTolerance(1, 2, 0.01, 0.01, 0.25), 1e-14);
        }

        [TestMethod]
        public void DivisionTolerance_TakesSquareRootOfExplicitFloor()
        {
            Assert.AreEqual(0.5, ErrorPropagation.DivisionTolerance(1, 2, 0.01, 0.01, 0.25), 1e-14);
        }

        [TestMethod]
        public void SumSquareTolerance_RejectsNaNUncertainty()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ErrorPropagation.SumSquareTolerance(double.NaN, 0.1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ErrorPropagation.SumSquareTolerance(0.1, double.NaN));
        }
    }
}
