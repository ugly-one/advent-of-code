using NUnit.Framework;
using Solutions.day6;
using System;
namespace day6
{
    [TestFixture()]
    public class ParserTests
    {
        [Test()]
        public void ToPoint_ReturnsPointWithCorrectCoordinates()
        {
            var point = "1, 6".ToPoint();
            Assert.AreEqual(point.X, 1);
            Assert.AreEqual(point.Y, 6);
        }
    }
}
