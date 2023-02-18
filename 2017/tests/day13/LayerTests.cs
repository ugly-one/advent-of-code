using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day13;
using System.Collections.Generic;

namespace tests.day13
{
    [TestClass]
    public class LayerTests
    {

        [TestMethod]
        public void Move_ReturnsTriangleNumbers_WhenRange5()
        {
            var result = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                result.Add(Layer.Move(5, i));
            }

            Assert.AreEqual(0, result[0]);
            Assert.AreEqual(1, result[1]);
            Assert.AreEqual(2, result[2]);
            Assert.AreEqual(3, result[3]);
            Assert.AreEqual(4, result[4]);
            Assert.AreEqual(3, result[5]);
            Assert.AreEqual(2, result[6]);
            Assert.AreEqual(1, result[7]);
            Assert.AreEqual(0, result[8]);
            Assert.AreEqual(1, result[9]);
        }
        [TestMethod]
        public void Move_ReturnsTriangleNumbers_WhenRange2()
        {
            var result = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                result.Add(Layer.Move(2, i));
            }

            Assert.AreEqual(0, result[0]);
            Assert.AreEqual(1, result[1]);
            Assert.AreEqual(0, result[2]);
            Assert.AreEqual(1, result[3]);
            Assert.AreEqual(0, result[4]);
            Assert.AreEqual(1, result[5]);
        }

        [TestMethod]
        public void Move_ReturnsTriangleNumbers_WhenRange3()
        {
            var result = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                result.Add(Layer.Move(3, i));
            }

            Assert.AreEqual(0, result[0]);
            Assert.AreEqual(1, result[1]);
            Assert.AreEqual(2, result[2]);
            Assert.AreEqual(1, result[3]);
            Assert.AreEqual(0, result[4]);
            Assert.AreEqual(1, result[5]);
        }
    }
}
