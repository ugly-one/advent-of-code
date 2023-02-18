using System;
using System.Collections.Generic;
using System.Linq;

namespace solutions.day3
{
    public static class ValueProvider
    {
        public static int GetValue (Position currentPosition, IEnumerable<Square> squares){

            IEnumerable<Square> closeSquares = squares.Where(s => s.IsNear(currentPosition)).ToList();
            var sum = 0;
            foreach(Square s in closeSquares){
                sum += s.Value;
            }

            return sum;
        }

        private static bool IsNear(this Square square, Position position){
            
            if (Math.Abs(square.Position.X - position.X) > 1) return false;
            if (Math.Abs(square.Position.Y - position.Y) > 1) return false;
            
            return true;
        }
    }
}