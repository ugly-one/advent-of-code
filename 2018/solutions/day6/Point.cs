using System;
using System.Diagnostics;

namespace Solutions.day6
{
    [DebuggerDisplay("X:{X} Y:{Y}")]
    public class Point
    {
        public readonly uint X;
        public readonly uint Y;

        public Point(uint x, uint y)
        {
            X = x;
            Y = y;
        }
        public static bool operator ==(Point p1, object p2)
        {
            return Equals(p1, p2);
        }

        public static bool operator !=(Point p1, object p2)
        {
            return !Equals(p1, p2);

        }
        public override bool Equals(object other)
        {
            if (other is null) return false;
            if (other.GetType() != this.GetType()) return false;

            var otherPoint = other as Point;
            if (this.X == otherPoint.X && this.Y == otherPoint.Y) return true;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public long GetDistance(long x, long y)
        {
            return Math.Abs(X - x) + Math.Abs(Y - y);
        }
    }
}
