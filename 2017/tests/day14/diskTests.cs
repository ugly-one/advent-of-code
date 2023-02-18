using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day14;
using System.Linq;

namespace tests.day14
{
    [TestClass]
    public class diskTests
    {
        [TestMethod]
        public void getRow_row0()
        {
            Assert.AreEqual("11010100", DiskDefragmentor.GetRowRepresantation("flqrgnkx", 0).Substring(0, 8));
        }

        [TestMethod]
        public void getRow_row1()
        {
            Assert.AreEqual("01010101", DiskDefragmentor.GetRowRepresantation("flqrgnkx", 1).Substring(0, 8));
        }

        [TestMethod]
        public void getDiskRepresentation()
        {
            DiskCell[][] rows = DiskDefragmentor.GetDiskRepresentation("flqrgnkx");
            int usedCount = DiskDefragmentor.GetUsedCount(rows);
            
            Assert.AreEqual(8108, usedCount);
        }

        [TestMethod]
        public void getDiskRepresentation_realData()
        {
            DiskCell[][] rows = DiskDefragmentor.GetDiskRepresentation("hfdlxzhv");
            int usedCount = DiskDefragmentor.GetUsedCount(rows);

            Assert.AreEqual(8230, usedCount);
        }

        [TestMethod]
        public void getDiskRepresentation_marksXY_Corrrectly()
        {
            DiskCell[][] rows = DiskDefragmentor.GetDiskRepresentation("flqrgnkx");

            Assert.AreEqual(rows[0][7].used, false);
            Assert.AreEqual(rows[0][7].x, 7);
            Assert.AreEqual(rows[0][7].y, 0);
        }
    }
}
