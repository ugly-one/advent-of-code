using System;
using System.Collections.Generic;
using System.Linq;

namespace solutions.day12
{
    public class RouteFinder
    {
        private List<Point> points;
        public RouteFinder(List<Point> argPoints)
        {
            points = argPoints.ToList();
        }
        
        public RouteResult FindRoute(int startId, int endId, List<Point> currentRoute)
        {
            if (startId == endId) return new RouteResult(true, new List<Point>());

            var startPoint = points.Find(p => p.ID == startId);
            if (startPoint == null) return NoRouteFound();

            foreach (var connection in startPoint.Connections)
            {
                if (currentRoute.FirstOrDefault(p => p.ID == connection.ID) != null) continue;
                var currentRoute2 = currentRoute.ToList();
                currentRoute2.Add(startPoint);
                var result = FindRoute(connection.ID, endId, currentRoute2);
                if (result.RouteExists) return new RouteResult(result.RouteExists, currentRoute2);
                currentRoute2.Remove(startPoint);
            }

            return NoRouteFound();
        }

        private RouteResult NoRouteFound()
        {
            return new RouteResult(false, new List<Point>());
        }
    }
}
