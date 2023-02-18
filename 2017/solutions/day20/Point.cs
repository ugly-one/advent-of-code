using System;

namespace solutions.day20
{
    public struct Point
    {
        public readonly long X;
        public readonly long Y;
        public readonly long Z;
        
        public Point(long x, long y, long z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            Point p = (Point)obj;
            return (X == p.X) && (Y == p.Y) && (Z == p.Z);
        }
        public override int GetHashCode()
        {
            return (int)(X ^ Y ^ Z);
        }
    }

    public static class PointExtensions
    {
        public static Point Add(this Point argPoint, Point argPointToAdd)
        {
            return new Point(
                argPoint.X + argPointToAdd.X,
                argPoint.Y + argPointToAdd.Y,
                argPoint.Z + argPointToAdd.Z
            );
        }

        public static long GetManhatanDistance(this Point point)
        {
            return Math.Abs(point.X) + Math.Abs(point.Y) + Math.Abs(point.Z);
        }
    }
}
