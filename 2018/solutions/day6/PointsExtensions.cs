using System;
using System.Collections.Generic;
using System.Linq;

namespace Solutions.day6
{
    public static class PointsExtensions
    {

        public static Borders FindBorders(this IEnumerable<Point> points)
        {
            var topBorder = -1; // smallest Y
            var bottomBorder = -1; // biggest Y
            var leftBorder = -1; // smallest X
            var rightBorder = -1; // biggest X

            foreach (var point in points)
            {
                // top - bottom
                if (topBorder == -1 || point.Y <= topBorder)
                    topBorder = (int)point.Y;
                else if (bottomBorder == -1 || point.Y >= bottomBorder)
                    bottomBorder = (int)point.Y;

                // left - right
                if (leftBorder == -1 || point.X <= leftBorder)
                    leftBorder = (int)point.X;
                else if (rightBorder == -1 || point.X >= rightBorder)
                    rightBorder = (int)point.X;
            }

            return new Borders(
                (uint)topBorder,
                (uint)bottomBorder,
                (uint)leftBorder,
                (uint)rightBorder);

        }
    }
}
