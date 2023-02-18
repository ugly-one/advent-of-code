using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day19;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tests.day19
{
    [TestClass]
    public class PathSolverTests
    {
        [TestMethod]
        public void test()
        {
            var map = MapReader.Read("../../../day19/TextFile1.txt");
            var startingPoint = MapReader.FindStart(map);
            var startingDirection = Direction.Down;

            var pathSolver = new PathSolver(map, startingDirection, startingPoint.First());

            var result = pathSolver.Walk();

            Assert.AreEqual("ABCDEF", result.path);
            Assert.AreEqual(38, result.steps);
        }

        [TestMethod]
        public void test2()
        {
            var map = MapReader.Read("../../../day19/TextFile2.txt");
            var startingPoint = MapReader.FindStart(map);
            var startingDirection = Direction.Down;

            var pathSolver = new PathSolver(map, startingDirection, startingPoint.First());

            var result = pathSolver.Walk();

            Assert.AreEqual("PBAZYFMHT", result.path);
            Assert.AreEqual(16072, result.steps);
        }
    }
}
