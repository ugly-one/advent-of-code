using System.Collections.Generic;
using System.IO;
using solutions.day2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests.day2
{
    [TestClass]
    public class day2Tests
    {
        [TestMethod]
        public void TestFirstRow()
        {
            var class1 = new Class1();
            Assert.AreEqual(4, class1.FindFirstEvenDivider(new List<int> { 2, 5, 8, 9 }));
        }

        [TestMethod]
        public void TestSecondRow()
        {
            var class1 = new Class1();
            Assert.AreEqual(3, class1.FindFirstEvenDivider(new List<int> { 3, 4, 7, 9 }));
        }

        [TestMethod]
        public void TestThirdRow()
        {
            var class1 = new Class1();
            Assert.AreEqual(2, class1.FindFirstEvenDivider(new List<int> { 3, 5, 6, 8 }));
        }

        [TestMethod]
        public void TestMethod()
        {
            var class1 = new Class1();
            Assert.AreEqual(294, class1.getCheckSum(File.ReadAllLines("../../../day2/input.txt")));
        }
    }
}
