using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier.Tests
{
    [TestClass]
    public class MultiplierTest
    {
        [TestMethod]
        public void MultiplyMatrix3On3Test()
        {
            TestMatrix3On3(new MatricesMultiplier());
            TestMatrix3On3(new MatricesMultiplierParallel());

            TestMatrix5On5(new MatricesMultiplier());
            TestMatrix5On5(new MatricesMultiplierParallel());
        }

        [TestMethod]
        public void ParallelEfficiencyTest()
        {          
            var firstMatrix = new Matrix(100, 100, true);
            var secondMatrix = new Matrix(100, 100, true);

            var syncWatch = Stopwatch.StartNew();
            new MatricesMultiplier().Multiply(firstMatrix, secondMatrix);
            syncWatch.Stop();

            var parallelWatch = Stopwatch.StartNew();
            new MatricesMultiplierParallel().Multiply(firstMatrix, secondMatrix);
            parallelWatch.Stop();

            Assert.IsTrue(parallelWatch.ElapsedMilliseconds < syncWatch.ElapsedMilliseconds);
        }

        #region private methods

        void TestMatrix3On3(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(3, 3);
            m1.SetElement(0, 0, 34);
            m1.SetElement(0, 1, 2);
            m1.SetElement(0, 2, 6);

            m1.SetElement(1, 0, 5);
            m1.SetElement(1, 1, 4);
            m1.SetElement(1, 2, 54);

            m1.SetElement(2, 0, 2);
            m1.SetElement(2, 1, 9);
            m1.SetElement(2, 2, 8);

            var m2 = new Matrix(3, 3);
            m2.SetElement(0, 0, 12);
            m2.SetElement(0, 1, 52);
            m2.SetElement(0, 2, 85);

            m2.SetElement(1, 0, 5);
            m2.SetElement(1, 1, 5);
            m2.SetElement(1, 2, 54);

            m2.SetElement(2, 0, 5);
            m2.SetElement(2, 1, 8);
            m2.SetElement(2, 2, 9);

            var multiplied = matrixMultiplier.Multiply(m1, m2);
            Assert.AreEqual(448, multiplied.GetElement(0, 0));
            Assert.AreEqual(1826, multiplied.GetElement(0, 1));
            Assert.AreEqual(3052, multiplied.GetElement(0, 2));

            Assert.AreEqual(350, multiplied.GetElement(1, 0));
            Assert.AreEqual(712, multiplied.GetElement(1, 1));
            Assert.AreEqual(1127, multiplied.GetElement(1, 2));

            Assert.AreEqual(109, multiplied.GetElement(2, 0));
            Assert.AreEqual(213, multiplied.GetElement(2, 1));
            Assert.AreEqual(728, multiplied.GetElement(2, 2));
        }

        void TestMatrix5On5(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(5, 5);
            m1.SetElement(0, 0, 1);
            m1.SetElement(0, 1, 1);
            m1.SetElement(0, 2, 2);
            m1.SetElement(0, 3, 2);
            m1.SetElement(0, 4, 2);

            m1.SetElement(1, 0, 1);
            m1.SetElement(1, 1, 1);
            m1.SetElement(1, 2, 2);
            m1.SetElement(1, 3, 2);
            m1.SetElement(1, 4, 2);

            m1.SetElement(2, 0, 1);
            m1.SetElement(2, 1, 1);
            m1.SetElement(2, 2, 2);
            m1.SetElement(2, 3, 2);
            m1.SetElement(2, 4, 2);

            m1.SetElement(3, 0, 1);
            m1.SetElement(3, 1, 1);
            m1.SetElement(3, 2, 2);
            m1.SetElement(3, 3, 2);
            m1.SetElement(3, 4, 2);

            m1.SetElement(4, 0, 1);
            m1.SetElement(4, 1, 1);
            m1.SetElement(4, 2, 2);
            m1.SetElement(4, 3, 2);
            m1.SetElement(4, 4, 2);

            var m2 = new Matrix(5, 5);
            m2.SetElement(0, 0, 2);
            m2.SetElement(0, 1, 3);
            m2.SetElement(0, 2, 1);
            m2.SetElement(0, 3, 1);
            m2.SetElement(0, 4, 0);

            m2.SetElement(1, 0, 2);
            m2.SetElement(1, 1, 3);
            m2.SetElement(1, 2, 1);
            m2.SetElement(1, 3, 1);
            m2.SetElement(1, 4, 0);

            m2.SetElement(2, 0, 2);
            m2.SetElement(2, 1, 3);
            m2.SetElement(2, 2, 1);
            m2.SetElement(2, 3, 1);
            m2.SetElement(2, 4, 0);

            m2.SetElement(3, 0, 2);
            m2.SetElement(3, 1, 3);
            m2.SetElement(3, 2, 1);
            m2.SetElement(3, 3, 1);
            m2.SetElement(3, 4, 0);

            m2.SetElement(4, 0, 2);
            m2.SetElement(4, 1, 3);
            m2.SetElement(4, 2, 1);
            m2.SetElement(4, 3, 1);
            m2.SetElement(4, 4, 0);

            var multiplied = matrixMultiplier.Multiply(m1, m2);
            Assert.AreEqual(16, multiplied.GetElement(0, 0));
            Assert.AreEqual(24, multiplied.GetElement(0, 1));
            Assert.AreEqual(8, multiplied.GetElement(0, 2));
            Assert.AreEqual(8, multiplied.GetElement(0, 3));
            Assert.AreEqual(0, multiplied.GetElement(0, 4));

            Assert.AreEqual(16, multiplied.GetElement(1, 0));
            Assert.AreEqual(24, multiplied.GetElement(1, 1));
            Assert.AreEqual(8, multiplied.GetElement(1, 2));
            Assert.AreEqual(8, multiplied.GetElement(1, 3));
            Assert.AreEqual(0, multiplied.GetElement(1, 4));

            Assert.AreEqual(16, multiplied.GetElement(2, 0));
            Assert.AreEqual(24, multiplied.GetElement(2, 1));
            Assert.AreEqual(8, multiplied.GetElement(2, 2));
            Assert.AreEqual(8, multiplied.GetElement(2, 3));
            Assert.AreEqual(0, multiplied.GetElement(2, 4));

            Assert.AreEqual(16, multiplied.GetElement(3, 0));
            Assert.AreEqual(24, multiplied.GetElement(3, 1));
            Assert.AreEqual(8, multiplied.GetElement(3, 2));
            Assert.AreEqual(8, multiplied.GetElement(3, 3));
            Assert.AreEqual(0, multiplied.GetElement(3, 4));

            Assert.AreEqual(16, multiplied.GetElement(4, 0));
            Assert.AreEqual(24, multiplied.GetElement(4, 1));
            Assert.AreEqual(8, multiplied.GetElement(4, 2));
            Assert.AreEqual(8, multiplied.GetElement(4, 3));
            Assert.AreEqual(0, multiplied.GetElement(4, 4));
        }

        #endregion
    }
}
