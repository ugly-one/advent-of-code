using System.Collections.Generic;
using System.Linq;

namespace solutions.day12
{
    public class RouteResult
    {
        public bool RouteExists;
        public List<Point> Route;

        public RouteResult(bool routeExists, List<Point> route)
        {
            RouteExists = routeExists;
            Route = route.ToList();
        }
    }
}
