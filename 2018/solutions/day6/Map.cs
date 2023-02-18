using System;
using System.Collections.Generic;
using System.Linq;

namespace Solutions.day6
{
    public class Map
    {
        private readonly Point[] points;
        private readonly Point[][] grid; // each element is a Point that is closest to this element 
        private readonly uint gridWidth;
        private readonly uint gridHeight;
        private Borders borders;

        public Map(Point[] points)
        {
            this.points = points;
            borders = points.FindBorders();

            gridWidth = borders.Right - borders.Left + 1;
            gridHeight = borders.Bottom - borders.Top + 1;

            //initialize the grid
            grid = new Point[gridWidth][];
            for (int i = 0; i < gridWidth; i++)
            {
                grid[i] = new Point[gridHeight];
            }
        }

        public uint GetBiggestArea()
        {
            var areas = new Dictionary<Point, uint>();
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    Point closestPoint = GetTheClosestPoint(x + borders.Left, y + borders.Top, points);
                    grid[x][y] = closestPoint;
                    if (closestPoint is null) continue;
                    if (areas.ContainsKey(closestPoint))
                    {
                        areas[closestPoint]++;
                    }
                    else
                    {
                        areas.Add(closestPoint, 1);
                    }
                }
            }

            // go through borders and remove areas that touches the borders - those are infinite
            // go through top border
            for (int x = 0; x < gridWidth; x++)
            {
                var point = grid[x][0];
                if (!(point is null) && areas.ContainsKey(point))
                    areas.Remove(point);
            }
            // go through bottom border
            for (int x = 0; x < gridWidth; x++)
            {
                var point = grid[x][gridHeight-1];
                if (!(point is null) && areas.ContainsKey(point))
                    areas.Remove(point);
            }
            // go through left border
            for (int y = 0; y < gridHeight; y++)
            {
                var point = grid[0][y];
                if (!(point is null) && areas.ContainsKey(point))
                    areas.Remove(point);
            }
            // go through bottom border
            for (int y = 0; y < gridHeight; y++)
            {
                var point = grid[gridWidth - 1][y];
                if (!(point is null) && areas.ContainsKey(point))
                    areas.Remove(point);
            }


            return areas.Max(a => a.Value);
        }

        public ulong RegionSize(uint maxTotalDistance)
        {
            ulong regionSize = 0;
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    var totalDistance = CalculateTotalDistance(x, y, points);
                    if (totalDistance < maxTotalDistance)
                        regionSize++;
                }
            }

            return regionSize;
        }

        private ulong CalculateTotalDistance(int x, int y, Point[] points)
        {
            ulong distance = 0;
            foreach (var point in points)
            {
                distance += (ulong)point.GetDistance(x, y);
            }
            return distance;
        }

        /// <summary>
        /// Returns one of the point from provided points that are the closest to the given coordinate.
        /// </summary>
        /// <returns>The the closest point.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="points">Points.</param>
        public Point GetTheClosestPoint(long x, long y, IEnumerable<Point> points)
        {
            long smallestDistance = -1;
            Point closestPoint = null;
            foreach (var point in points)
            {
                var dis = point.GetDistance(x, y);
                if (smallestDistance == -1 || dis < smallestDistance)
                {
                    smallestDistance = dis;
                    closestPoint = point;
                }
                else if (dis == smallestDistance)
                {
                    closestPoint = null; // in case 2 points are same distance to the element - do not mark it
                }
            }

            return closestPoint;
        }
    }
}
