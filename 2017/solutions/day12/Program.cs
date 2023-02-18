using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace solutions.day12
{
    class Program
    {
        static PointsManager pointsManager;

        static void PrintConnections(IEnumerable<Point> points)
        {
            foreach (var point in points)
            {
                Console.Write(point.ID + "connected with => ");
                foreach (var connection in point.Connections)
                {
                    Console.Write(connection.ID);
                }
                Console.WriteLine();
            }
        }

        static void ReadPoints()
        {
            using (StreamReader sr = new StreamReader("realTestData.txt"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var split = line.Split( new string[] {"<->"}, StringSplitOptions.None);
                    var rootId = split[0].Trim();

                    // add connections
                    var connectionsString = split[1].Split(',');
                    foreach (var connectionIdNotTrimmed in connectionsString)
                    {
                        var connectionId = connectionIdNotTrimmed.Trim();
                        Int32.TryParse(rootId, out int idRootInt);
                        Int32.TryParse(connectionId, out int connectionIdInt);
                        pointsManager.AddConnection(idRootInt, connectionIdInt);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            pointsManager = new PointsManager();
            ReadPoints();
            PrintConnections(pointsManager.Points);

            var routeFinder = new RouteFinder(pointsManager.Points);

            var pointsToTest = pointsManager.Points;
            var sum = 0;
            foreach (var pointToTest in pointsToTest)
            {
                var result = routeFinder.FindRoute(pointToTest.ID, 0, new List<Point>());
                if (result.RouteExists) sum++;
                Console.Write($"finding route between 0 and {pointToTest.ID} -> ");
                Console.WriteLine(result.RouteExists.ToString());
            }

            Console.WriteLine(sum);
        }
    }
}
