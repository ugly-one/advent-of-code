using System.Collections.Generic;

namespace solutions.day12
{
    public class Point
    {
        public int ID { get; }
        public List<Point> Connections;

        public Point(int argId)
        {
            ID = argId;
            Connections = new List<Point>();
        }
    }
}
