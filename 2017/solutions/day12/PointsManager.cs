using System.Collections.Generic;
using System.Linq;

namespace solutions.day12
{
    public class PointsManager
    {
        private List<Point> points = new List<Point>();

        public List<Point> Points {
            get
            {
                return points.ToList();
            }
        }

        public Point Create(int id)
        {
            var search = points.Where(p => p.ID == id);
            Point point;
            if (search.Count() == 0)
            {
                point = new Point(id);
                points.Add(point);
            }
            else
            {
                point = search.FirstOrDefault();
            }

            return point;
        }

        public void AddConnection(int sourceId, int connectionId)
        {
            Point connectionPoint = Create(connectionId);
            Point sourcePoint = Create(sourceId);
            sourcePoint.Connections.Add(connectionPoint);
        }
    }
}
