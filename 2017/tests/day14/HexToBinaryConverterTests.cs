using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day14;

namespace tests.day14
{
    [TestClass]
    public class HexToBinaryConverterTests
    {
        [TestMethod]
        public void Convert_1()
        {
            Assert.AreEqual("0001", HexToBinaryConverter.Convert("1"));
        }

        [TestMethod]
        public void Convert_8()
        {
            Assert.AreEqual("1000", HexToBinaryConverter.Convert("8"));
        }

        [TestMethod]
        public void Convert_a()
        {
            Assert.AreEqual("1010", HexToBinaryConverter.Convert("a"));
        }

        [TestMethod]
        public void Convert_f()
        {
            Assert.AreEqual("1111", HexToBinaryConverter.Convert("f"));
        }

        [TestMethod]
        public void Convert_a0c2017()
        {
            Assert.AreEqual("1010000011000010000000010111", HexToBinaryConverter.Convert("a0c2017"));
        }
    }
}
