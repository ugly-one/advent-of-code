using System;
using System.Collections.Generic;
using System.Linq;

namespace solutions.day3
{
    public class Part2
    {
        public List<Square> GeneratePuzzle(int target)
        {
            var squares = new List<Square>();
            var currentPosition = new Position(0, 0);
            var currentDirection = Direction.XRIGHTONCE;
            var currentWallSize = 1;
            var currentValue = 1;
            bool end = false;
            squares.Add(new Square(currentPosition, currentValue));
            do
            {
                bool canMove = true;
                do
                {
                    try
                    {
                        var newPosition = move(currentPosition, currentDirection, currentWallSize);
                        var newValue = ValueProvider.GetValue(newPosition,squares);
                        if (newValue >= target)
                        {
                            canMove = false;
                            end = true;
                        }
                        squares.Add(new Square(newPosition, newValue));
                        currentPosition = new Position(newPosition.X, newPosition.Y);
                    }
                    catch (ArgumentException)
                    {
                        if (currentDirection == Direction.XRIGHTONCE)
                        {
                            currentDirection = Direction.YUP;
                        }
                        else
                        {
                            canMove = false;
                        }
                    }
                } while (canMove);

                switch (currentDirection)
                {
                    case Direction.XRIGHT:
                        currentWallSize += 2;
                        currentDirection = Direction.XRIGHTONCE;
                        break;
                    case Direction.YUP:
                        currentDirection = Direction.XLEFT;
                        break;
                    case Direction.XLEFT:
                        currentDirection = Direction.YDOWN;
                        break;
                    case Direction.YDOWN:
                        currentDirection = Direction.XRIGHT;
                        break;
                    default:
                        break;
                }
            } while (!end);

            return squares;
        }

        public Position move(Position currentPosition, Direction direction, int wallSize)
        {
            var max = (wallSize - 1) / 2;
            var newPosition = new Position(currentPosition.X, currentPosition.Y);
            switch (direction)
            {
                case Direction.XRIGHTONCE:
                    if (currentPosition.X + 1 > max) throw new ArgumentException("Cannot move");
                    newPosition.X += 1;
                    break;
                case Direction.XRIGHT:
                    if (currentPosition.X + 1 > max) throw new ArgumentException("Cannot move");
                    newPosition.X += 1;
                    break;
                case Direction.XLEFT:
                    if (currentPosition.X - 1 < -max) throw new ArgumentException("Cannot move");
                    newPosition.X -= 1;
                    break;
                case Direction.YUP:
                    if (currentPosition.Y + 1 > max) throw new ArgumentException("Cannot move");
                    newPosition.Y += 1;
                    break;
                case Direction.YDOWN:
                    if (currentPosition.Y - 1 < -max) throw new ArgumentException("Cannot move");
                    newPosition.Y -= 1;
                    break;
                default:
                    throw new ArgumentException("Cannot move");
            }
            return newPosition;
        }

    }
}
