using Common;
using NUnit.Framework;
using Solutions.day6;
using System;
using System.Linq;

namespace day6
{
    [TestFixture()]
    public class MapTests
    {
        Point[] points;
        Map map;

        [SetUp]
        public void Inite()
        {
            points = new Point[]
            {
                new Point(1, 1),
                new Point(1, 6),
                new Point(8, 3),
                new Point(3, 4),
                new Point(5, 5),
                new Point(8, 9)
            };
            map = new Map(points);

        }
        [Test()]
        public void BiggestAreaTestInput()
        {
            var area = map.GetBiggestArea();
            Assert.AreEqual(17, area);
        }

        [Test()]
        public void ClosestPoint()
        {
            var closestPoint = map.GetTheClosestPoint(5,2, points);
            Assert.AreEqual(points[4], closestPoint);
        }

        [Test()]
        public void BiggestAreaPart1Input()
        {
            var points = FileReader.Read("../../day6/input.txt").Select(l => l.ToPoint()).ToArray();
            var map = new Map(points);

            var area = map.GetBiggestArea();

            Assert.AreEqual(2342, area);
        }

        [Test()]
        public void RegionSizeWhereDistanceLessThan32_part2testInput()
        {
            var regionSize = map.RegionSize(32);
            Assert.AreEqual(16, regionSize);
        }

        [Test()]
        public void RegionSizeWhereDistanceLessThan1000_Is43302_part2Solution()
        {
            var points = FileReader.Read("../../day6/input.txt").Select(l => l.ToPoint()).ToArray();
            var map = new Map(points);

            var regionSize = map.RegionSize(10000);
            Assert.AreEqual(43302, regionSize);
        }
    }
}
