using System;
using solutions.day19;

namespace tests.day19
{
    internal class PathSolver
    {
        private char[][] map;
        private Direction direction;
        private Point currentPoint;
        

        public PathSolver(char[][] map, Direction startingDirection, Point startingPoint)
        {
            this.map = map;
            this.direction = startingDirection;
            this.currentPoint = startingPoint;
        }

        internal (string path, int steps) Walk()
        {
            string result = "";
            int stepsCounter = 0;
            while(direction != Direction.Stop)
            {
                stepsCounter++;
                if (Char.IsLetter(map[currentPoint.Y][currentPoint.X]))
                    result += map[currentPoint.Y][currentPoint.X];

                direction = Mover.Move(MapReader.GetWindow(map, currentPoint), direction);
                currentPoint = currentPoint.Move(direction);
            }

            return (result, stepsCounter);
        }
    }
}