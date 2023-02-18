using System;
using System.Collections.Generic;

namespace solutions.day19
{
    public static class Mover
    {
        private static bool CanStepHereFromUpOrDown(char argChar)
        {
            return argChar != '-' && argChar != 'x' && argChar != ' ';
        }

        private static bool CanStepHereFromLeftOrRight(char argChar)
        {
            return argChar != '|' && argChar != 'x' && argChar != ' ';
        }

        private static bool CanGoDown(char[][] currentWindow)
        {
            if (!CanStepHereFromUpOrDown(currentWindow[3][2]))
            {
                if (currentWindow[3][2] == '-')
                    return CanStepHereFromUpOrDown(currentWindow[4][2]);
                else
                    return false;
            }
            return true;
        }

        private static bool CanGoUp(char[][] currentWindow)
        {
            if (!CanStepHereFromUpOrDown(currentWindow[1][2]))
            {
                if (currentWindow[1][2] == '-')
                    return CanStepHereFromUpOrDown(currentWindow[0][2]);
                else
                    return false;
            }
            return true;
        }

        private static bool CanGoRight(char[][] currentWindow)
        {
            if (!CanStepHereFromLeftOrRight(currentWindow[2][3]))
            {
                if (currentWindow[2][3] == '|')
                    return CanStepHereFromLeftOrRight(currentWindow[2][4]);
                else
                    return false;
            }
            return true;
        }

        private static bool CanGoLeft(char[][] currentWindow)
        {
            if (!CanStepHereFromLeftOrRight(currentWindow[2][1]))
            {
                if (currentWindow[2][1] == '|')
                    return CanStepHereFromLeftOrRight(currentWindow[2][0]);
                else
                    return false;
            }
            return true;
        }

        public static Direction Move(char[][] currentWindow, Direction argDirection)
        {
            if (argDirection == Direction.Down)
            {
                if (CanGoDown(currentWindow))
                    return argDirection;
                else
                {
                    if (CanGoLeft(currentWindow))
                        return Direction.Left;
                    if (CanGoRight(currentWindow))
                        return Direction.Right;
                }
            }

            if (argDirection == Direction.Up)
            {
                if (CanGoUp(currentWindow))
                    return argDirection;
                else
                {
                    if (CanGoLeft(currentWindow))
                        return Direction.Left;
                    if (CanGoRight(currentWindow))
                        return Direction.Right;
                }
            }

            if (argDirection == Direction.Left)
            {
                if (CanGoLeft(currentWindow))
                    return argDirection;
                else
                {
                    if (CanGoUp(currentWindow))
                        return Direction.Up;
                    if (CanGoDown(currentWindow))
                        return Direction.Down;
                }
            }

            if (argDirection == Direction.Right)
            {
                if (CanGoRight(currentWindow))
                    return argDirection;
                else
                {
                    if (CanGoUp(currentWindow))
                        return Direction.Up;
                    if (CanGoDown(currentWindow))
                        return Direction.Down;
                }
            }
             
            return Direction.Stop;
        }
    }
}