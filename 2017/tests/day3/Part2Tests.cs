using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System;
using solutions.day3;

namespace tests.day3
{
    [TestClass]
    public class Part2Tests
    {
        Part2 _sut;
        [TestInitialize]
        public void init()
        {
            _sut = new Part2();
        }
        [TestMethod]
        public void Move_RIGHT_increasesX_WhenWallSizeBigEnough()
        {
            var pos = new Position(0, 0);
            Assert.AreEqual(1, _sut.move(pos, Direction.XRIGHT, 3).X);
        }

        [TestMethod]
        public void Move_YUP_increasesY_WhenWallSizeBigEnough()
        {
            var pos = new Position(0, 0);
            Assert.AreEqual(1, _sut.move(pos, Direction.YUP, 3).Y);
        }

        [TestMethod]
        public void Move_YDOWN_decreasesY_WhenWallSizeBigEnough()
        {
            var pos = new Position(-1, 1);
            Assert.AreEqual(0, _sut.move(pos, Direction.YDOWN, 3).Y);
        }

        [TestMethod]
        public void Move_XLEFT_decreasesX_WhenWallSizeBigEnough()
        {
            var pos = new Position(0, 1);
            Assert.AreEqual(-1, _sut.move(pos, Direction.XLEFT, 3).X);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),"Cannot move")]
        public void Move_XLEFT_throws_WhenWallSizeTooSmall()
        {
            var pos = new Position(-1, 1);
            _sut.move(pos, Direction.XLEFT, 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),"Cannot move")]
        public void Move_XRIGHT_throws_WhenWallSizeTooSmall()
        {
            var pos = new Position(1, 1);
            _sut.move(pos, Direction.XRIGHT, 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),"Cannot move")]
        public void Move_YDOWN_throws_WhenWallSizeTooSmall()
        {
            var pos = new Position(-1, -1);
            _sut.move(pos, Direction.YDOWN, 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),"Cannot move")]
        public void Move_YUP_throws_WhenWallSizeTooSmall()
        {
            var pos = new Position(-1, 1);
            _sut.move(pos, Direction.YUP, 3);
        }

        [TestMethod]
        public void Generate_creates6Elements_When10IsTheLimit() 
            => Assert.AreEqual(6, _sut.GeneratePuzzle(10).Count);

        [TestMethod]
        public void Generate_ReturnsLastElementWithCorrectPosition()
        {
            var squares = _sut.GeneratePuzzle(11);
            Assert.AreEqual(-1, squares.Last().Position.X);
            Assert.AreEqual(-1, squares.Last().Position.Y);
        }

        [TestMethod]
        public void Generate_LastValueIs266330_When265149IsTheLimit(){
            Assert.AreEqual(266330, _sut.GeneratePuzzle(265149).Last().Value);
        } 

        [TestMethod]
        public void Generate_ReturnsLastValue747_When730IsTheLimit() 
            => Assert.AreEqual(747, _sut.GeneratePuzzle(730).Last().Value);

    }
}