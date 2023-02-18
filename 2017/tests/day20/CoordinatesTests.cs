using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day20;

namespace tests.day20
{
    [TestClass]
    public class CoordinatesTests
    {
        [TestMethod]
        public void GetManhatanDistance_When220_Returns4()
        {
            Point point = new Point(2, 2, 0);

            Assert.AreEqual(4, point.GetManhatanDistance());
        }

        [TestMethod]
        public void GetManhatanDistance_When5Minus4Minus10_Returns19()
        {
            Point point = new Point(5, -4, -10);

            Assert.AreEqual(19, point.GetManhatanDistance());
        }
    }
}
