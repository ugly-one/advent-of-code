using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day12;
using System.Collections.Generic;

namespace tests.day12
{
    [TestClass]
    public class RouteFinderTests
    {
        [TestMethod]
        public void FindRoute_ReturnsNoRoute_WhenNoConnection()
        {
            List<Point> points = new List<Point>();
            var point0 = new Point(0);
            var point1 = new Point(1);

            points.Add(point0);
            points.Add(point1);
            var sut = new RouteFinder(points);

            var result = sut.FindRoute(0, 1, new List<Point>());

            Assert.IsFalse(result.RouteExists);
        }

        [TestMethod]
        public void FindRoute_ReturnsRoute_WhenTwoConnectedPointsRequested()
        {
            List<Point> points = new List<Point>();
            var point0 = new Point(0);
            var point1 = new Point(1);

            points.Add(point0);
            points.Add(point1);

            point0.Connections.Add(point1);
            point1.Connections.Add(point0);

            var sut = new RouteFinder(points);

            var result = sut.FindRoute(0, 1, new List<Point>());

            Assert.IsTrue(result.RouteExists);
        }

        [TestMethod]
        public void FindRoute_ReturnsRoute_WhenTwoPointsDividedByOne()
        {
            List<Point> points = new List<Point>();
            var point0 = new Point(0);
            var point1 = new Point(1);
            var point2 = new Point(2);

            points.Add(point0);
            points.Add(point1);
            points.Add(point2);

            point0.Connections.Add(point1);
            point1.Connections.Add(point0);

            point1.Connections.Add(point2);
            point2.Connections.Add(point1);

            var sut = new RouteFinder(points);

            var result = sut.FindRoute(0, 2, new List<Point>());

            Assert.IsTrue(result.RouteExists);
        }
    }
}
