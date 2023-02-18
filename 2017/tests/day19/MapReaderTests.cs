using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day19;
using System.Linq;

namespace tests.day19
{
    [TestClass]
    public class MapReaderTests
    {
        private char[][] map;
        private const int windowSize = 5;

        [TestInitialize]
        public void Init()
        {
            map = MapReader.Read("../../../day19/TextFile1.txt");
        }

        [TestMethod]
        public void Reading()
        {
            Assert.AreEqual('A', map[2][5]);
        }

        [TestMethod]
        public void GettingFirstElement()
        {
            var start = MapReader.FindStart(map);

            Assert.AreEqual(new Point(5,0), start.First());
        }

        [TestMethod]
        public void GetWindow_SomewhereInTheCenterOfTheMap()
        {
            var window = MapReader.GetWindow(map, new Point(5, 2));
            char[][] expectedWindow = new char[windowSize][] {
                new char[]{ 'x', 'x', '|', 'x' , 'x' },
                new char[]{ 'x', 'x', '|', 'x' , 'x' },
                new char[]{ ' ', ' ', 'A', ' ' , ' ' },
                new char[]{ 'x', 'x', '|',  'x', 'x' },
                new char[]{ 'x', 'x', '|',  'x', 'x' },
            };

            AssertArraysEqual(expectedWindow, window);
        }

        [TestMethod]
        public void GetWindow_OnTheLeftEdge()
        {
            var window = MapReader.GetWindow(map, new Point(0, 2));
            char[][] expectedWindow = new char[windowSize][] {
                new char[] { 'x', 'x', ' ', 'x', 'x' },
                new char[] { 'x', 'x', ' ', 'x', 'x' },
                new char[] { 'x', 'x', ' ', ' ', ' ' },
                new char[] { 'x', 'x', ' ', 'x', 'x' },
                new char[] { 'x', 'x', ' ', 'x', 'x' },
            };

            AssertArraysEqual(expectedWindow, window);
        }
        [TestMethod]
        public void GetWindow_OnTheRightEdge()
        {
            var window = MapReader.GetWindow(map, new Point(15, 4));
            char[][] expectedWindow = new char[windowSize][] {
                new char[] { 'x', 'x', ' ', 'x', 'x' },
                new char[] { 'x', 'x', ' ', 'x', 'x' },
                new char[] { ' ', 'D', ' ', 'x', 'x' },
                new char[] { 'x', 'x', ' ', 'x', 'x' },
                new char[] { 'x', 'x', 'x', 'x', 'x' },
            };

            AssertArraysEqual(expectedWindow, window);
        }


        [TestMethod]
        public void GetWindow_OnTheTopEdge()
        {
            var window = MapReader.GetWindow(map, new Point(5, 0));
            char[][] expectedWindow = new char[windowSize][] {
                new char[] { 'x', 'x', 'x', 'x', 'x' },
                new char[] { 'x', 'x', 'x', 'x', 'x' },
                new char[] { ' ', ' ', '|', ' ', ' ' },
                new char[] { 'x', 'x', '|', 'x', 'x' },
                new char[] { 'x', 'x', 'A', 'x', 'x' },
            };

            AssertArraysEqual(expectedWindow, window);
        }

        [TestMethod]
        public void GetWindow_OnTheBottomEdge()
        {
            var window = MapReader.GetWindow(map, new Point(7, 5));
            char[][] expectedWindow = new char[windowSize][] {
                new char[] { 'x', 'x', '-', 'x' , 'x'},
                new char[] { 'x', 'x', ' ', 'x' , 'x'},
                new char[] { '+', 'B', '-', '+' , ' '},
                new char[] { 'x', 'x', 'x', 'x' , 'x'},
                new char[] { 'x', 'x', 'x', 'x' , 'x'},
            };

            AssertArraysEqual(expectedWindow, window);
        }

        private void AssertArraysEqual(char[][] expected, char[][] observed)
        {
            for (int i = 0; i < expected[0].Length; i++)
            {
                for (int j = 0; j < expected.Length; j++)
                {
                    Assert.AreEqual(expected[i][j], observed[i][j]);
                }
            }
        }
    }
}
