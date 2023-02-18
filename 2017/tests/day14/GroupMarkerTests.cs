using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day14;

namespace tests.day14
{
    [TestClass]
    public class GroupMarkerTests
    {
        [TestMethod]
        public void markGroups_marksFirstGroup()
        {
            DiskCell[][] rows = DiskDefragmentor.GetDiskRepresentation("flqrgnkx");
            var marker = new GroupMarker(rows);
            marker.MarkGroups();

            Assert.AreEqual(1, rows[0][0].group);
            Assert.AreEqual(1, rows[0][1].group);
            Assert.AreEqual(1, rows[1][1].group);
        }

        [TestMethod]
        public void markGroups_marksSecondGroup()
        {
            DiskCell[][] rows = DiskDefragmentor.GetDiskRepresentation("flqrgnkx");
            var marker = new GroupMarker(rows);
            marker.MarkGroups();

            Assert.AreEqual(2, rows[0][3].group);
            Assert.AreEqual(2, rows[1][3].group);
        }

        [TestMethod]
        public void markGroups_marksThirdGroup()
        {
            DiskCell[][] rows = DiskDefragmentor.GetDiskRepresentation("flqrgnkx");
            var marker = new GroupMarker(rows);
            marker.MarkGroups();

            Assert.AreEqual(3, rows[0][5].group);
            Assert.AreEqual(3, rows[1][5].group);
        }

        [TestMethod]
        public void markGroups_returns1242Groups_WhenTestData()
        {
            DiskCell[][] rows = DiskDefragmentor.GetDiskRepresentation("flqrgnkx");
            var marker = new GroupMarker(rows);
            var groupsAmount = marker.MarkGroups();

            Assert.AreEqual(1242, groupsAmount);
        }

        [TestMethod]
        public void markGroups_returns1103Groups_WhenRealData()
        {
            DiskCell[][] rows = DiskDefragmentor.GetDiskRepresentation("hfdlxzhv");
            var marker = new GroupMarker(rows);
            var groupsAmount = marker.MarkGroups();

            Assert.AreEqual(1103, groupsAmount);
        }
    }
}
