using Common;
using NUnit.Framework;
using Solutions.day6;
using System;
using System.Collections.Generic;
using System.Linq;

namespace day6
{
    [TestFixture()]
    public class PointsExtensionsTests
    {
        private IEnumerable<Point> GetTestPoints()
        {
            var input = FileReader.Read("../../day6/testInput.txt");
            return input.Select(line => line.ToPoint());
        }

        [Test()]
        public void GetBorders_ReturnsCorrectBorders_WhenTestInputProvided()
        {
            var points = GetTestPoints();

            var borders = points.FindBorders();

            Assert.AreEqual(1, borders.Top);
            Assert.AreEqual(9, borders.Bottom);
            Assert.AreEqual(1, borders.Left);
            Assert.AreEqual(8, borders.Right);
        }


    }
}
