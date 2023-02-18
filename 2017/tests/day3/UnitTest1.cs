using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day3;
using System.Linq;

namespace tests.day3
{
    [TestClass]
    public class UnitTest1
    {
        private Class1 _sut;

        [TestInitialize]
        public void init()
        {
            _sut = new Class1();
        }
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(3, _sut.getBigger(8));
        }

        [TestMethod]
        public void TestMethod2()
        {
            Assert.AreEqual(5, _sut.getBigger(25));
        }


        [TestMethod]
        public void TestMethod3()
        {
            Assert.AreEqual(5, _sut.getBigger(23));
        }

        [TestMethod]
        public void TestMethod()
        {
            Assert.AreEqual(515, _sut.getBigger(265149));
        }

        [TestMethod]
        public void distanceFromWallMiddle_Returns0_WhenInTheMiddle()
        {
            Assert.AreEqual(0, _sut.distanceFromWallMiddle(19, 5));
        }


        [TestMethod]
        public void distanceFromWallMiddle_ReturnsHalfWallSize_WhenInTheCorner()
        {
            Assert.AreEqual(2, _sut.distanceFromWallMiddle(17, 5));
        }

        [TestMethod]
        public void distanceFromWallMiddle_Returns1_When1FromMiddle()
        {
            Assert.AreEqual(1, _sut.distanceFromWallMiddle(14, 5));
        }

        [TestMethod]
        public void distanceFromWallMiddle_Returns2_When2FromMiddle()
        {
            Assert.AreEqual(2, _sut.distanceFromWallMiddle(30, 7));
        }

        [TestMethod]
        public void getDistanceFromMiddle_Returns2_WhenMiddleOf5SizeWall()
        {
            Assert.AreEqual(2, _sut.getDistanceFromMiddle(15));
        }

        [TestMethod]
        public void getDistanceFromMiddle_Returns4_WhenCornerOf5SizeWall()
        {
            Assert.AreEqual(4, _sut.getDistanceFromMiddle(13));
        }

        [TestMethod]
        public void getDistanceFromMiddle_Returns5_When30()
        {
            Assert.AreEqual(5, _sut.getDistanceFromMiddle(30));
        }
        [TestMethod]
        public void getDistanceFromMiddle_Returns438_When265149()
        {
            Assert.AreEqual(438, _sut.getDistanceFromMiddle(265149));
        }
    }
}
