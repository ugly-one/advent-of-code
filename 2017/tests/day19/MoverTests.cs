using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day19;

namespace tests.day19
{
    [TestClass]
    public class MoverTests
    {

        private char[][] map;

        [TestInitialize]
        public void Init()
        {
            map = MapReader.Read("../../../day19/TextFile1.txt");
        }

        [TestMethod]
        public void GoingDown()
        {
            char[][] currentWindow = MapReader.GetWindow(map, new Point(5, 0));
            Direction newDirection = Mover.Move(currentWindow, Direction.Down);

            Assert.AreEqual(Direction.Down, newDirection);
        }

        [TestMethod]
        public void GoingUp()
        {
            var currentWindow = new char[][]
            {
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { '+', 'B', '|', '+' , ' '},
                new char[] { 'x', 'x', 'x', 'x' , 'x'},
                new char[] { 'x', 'x', 'x', 'x' , 'x'},
            };

            Direction newDirection = Mover.Move(currentWindow, Direction.Up);

            Assert.AreEqual(Direction.Up, newDirection);
        }

        [TestMethod]
        public void GoingUp_KeepsTheDirection()
        {
            var currentWindow = new char[][]
            {
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { '+', 'B', '+', '+' , ' '},
                new char[] { 'x', 'x', 'x', 'x' , 'x'},
                new char[] { 'x', 'x', 'x', 'x' , 'x'},
            };

            Direction newDirection = Mover.Move(currentWindow, Direction.Up);

            Assert.AreEqual(Direction.Up, newDirection);
        }

        [TestMethod]
        public void TurnLeft_afterGoingDown_OnIntersection()
        {
            var currentWindow = new char[][]
            {
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { '+', '-', '+', ' ' , ' '},
                new char[] { 'x', 'x', 'x', 'x' , 'x'},
                new char[] { 'x', 'x', 'x', 'x' , 'x'},
            };
            Direction newDirection = Mover.Move(currentWindow, Direction.Down);

            Assert.AreEqual(Direction.Left, newDirection);
        }

        [TestMethod]
        public void Stop()
        {
            var currentWindow = new char[][]
            {
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { '+', ' ', '|', ' ' , ' '},
                new char[] { 'x', 'x', 'x', 'x' , 'x'},
                new char[] { 'x', 'x', 'x', 'x' , 'x'},
            };
            Direction newDirection = Mover.Move(currentWindow, Direction.Down);

            Assert.AreEqual(Direction.Stop, newDirection);
        }

        [TestMethod]
        public void KeepsGoingLeft()
        {
            var currentWindow = new char[][]
            {
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { '+', '-', '+', 'B' , ' '},
                new char[] { 'x', 'x', 'x', 'x' , 'x'},
                new char[] { 'x', 'x', 'x', 'x' , 'x'},
            };
            Direction newDirection = Mover.Move(currentWindow, Direction.Left);

            Assert.AreEqual(Direction.Left, newDirection);
        }

        [TestMethod]
        public void TakesOnlyVerticalPathAsIntersection_SoGoesUpHere()
        {
            var currentWindow = new char[][]
            {
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { '-', ' ', '+', 'B' , ' '},
                new char[] { 'x', 'x', ' ', 'x' , 'x'},
                new char[] { 'x', 'x', ' ', 'x' , 'x'},
            };
            Direction newDirection = Mover.Move(currentWindow, Direction.Left);

            Assert.AreEqual(Direction.Up, newDirection);
        }

        [TestMethod]
        public void TurnsUp()
        {
            var currentWindow = new char[][]
            {
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { '+', ' ', '+', '-' , ' '},
                new char[] { 'x', 'x', 'x', 'x' , 'x'},
                new char[] { 'x', 'x', 'x', 'x' , 'x'},
            };
            Direction newDirection = Mover.Move(currentWindow, Direction.Left);

            Assert.AreEqual(Direction.Up, newDirection);
        }

        [TestMethod]
        public void KeepsGoingInTheSameDirection_WhenSteppingOnALetter_AndItsPossible()
        {
            var currentWindow = new char[][]
            {
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { '+', '-', 'A', '-' , ' '},
                new char[] { 'x', 'x', 'x', 'x' , 'x'},
                new char[] { 'x', 'x', 'x', 'x' , 'x'},
            };
            Direction newDirection = Mover.Move(currentWindow, Direction.Left);

            Assert.AreEqual(Direction.Left, newDirection);
        }

        [TestMethod]
        public void StopsWhenHittingAWall()
        {
            var currentWindow = new char[][]
            {
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { '+', ' ', '+', ' ' , ' '},
                new char[] { 'x', 'x', 'x', 'x' , 'x'},
                new char[] { 'x', 'x', 'x', 'x' , 'x'},
            };
            Direction newDirection = Mover.Move(currentWindow, Direction.Down);

            Assert.AreEqual(Direction.Stop, newDirection);
        }

        [TestMethod]
        public void KeepsMovingInTheSameDirection_WhenWeirdIntersection()
        {
            var currentWindow = new char[][]
            {
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { '+', ' ', '|', ' ' , ' '},
                new char[] { 'x', 'x', '-', 'x' , 'x'},
                new char[] { 'x', 'x', '|', 'x' , 'x'},
            };
            Direction newDirection = Mover.Move(currentWindow, Direction.Down);

            Assert.AreEqual(Direction.Down, newDirection);
        }

        [TestMethod]
        public void KeepsMovingInTheSameDirection_WhenWeirdIntersection2()
        {
            var currentWindow = new char[][]
            {
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { '+', ' ', '-', ' ' , ' '},
                new char[] { 'x', 'x', '|', 'x' , 'x'},
                new char[] { 'x', 'x', '|', 'x' , 'x'},
            };
            Direction newDirection = Mover.Move(currentWindow, Direction.Down);

            Assert.AreEqual(Direction.Down, newDirection);
        }

        private char[][] CreateWindow(char[] row1, char[] row2, char[] row3)
        {
            return new char[3][]
            {
                row1,
                row2,
                row3,
            };
        }

    }
}
