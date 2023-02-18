using NUnit.Framework;
using Solutions.day6;
using System;
namespace day6
{
    [TestFixture()]
    public class PointTests
    {
        [Test()]
        public void Equals()
        {
            var point = new Point(3, 3);
            var point2 = new Point(3, 3);

            Assert.AreEqual(point, point2);
        }
        [Test()]
        public void NotEquals()
        {
            var point = new Point(3, 4);
            var point2 = new Point(4, 3);

            Assert.AreNotEqual(point, point2);
        }

        [Test()]
        public void NotEqualOperator()
        {
            var point = new Point(3, 4);
            var point2 = new Point(4, 3);

            Assert.IsTrue(point != point2);
        }

        [Test()]
        public void EqualOperator()
        {
            var point = new Point(1, 2);
            var point2 = new Point(1, 2);

            Assert.IsTrue(point == point2);
        }

        [Test()]
        public void EqualOperatorNull()
        {
            Point point = null;
            var point2 = new Point(1, 2);

            Assert.IsFalse(point == point2);
        }
    }
}
