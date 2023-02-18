using System;
using System.Collections.Generic;
using System.Text;

namespace solutions.day19
{
    public struct Point
    {
        public readonly int X;
        public readonly int Y;

        public Point(int argX, int argY)
        {
            X = argX;
            Y = argY;
        }
    }

    public static class PointExtensions
    {
        public static Point Move(this Point point, Direction argDirection)
        {
            switch (argDirection)
            {
                case Direction.Down:
                    return new Point(point.X, point.Y + 1);
                case Direction.Up:
                    return new Point(point.X, point.Y - 1);
                case Direction.Right:
                    return new Point(point.X + 1, point.Y);
                case Direction.Left:
                    return new Point(point.X - 1, point.Y);
                case Direction.Stop:
                    return point;
                default:
                    return point;
            }
        }
    }
}
