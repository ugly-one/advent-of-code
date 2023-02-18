using System;
using System.Collections.Generic;

namespace solutions.day3
{
    public class Class1
    {
        public int getBigger(int number){

            foreach (var item in NumbersGenerator.getOddNumbers())
            {
                if (item * item >= number){
                    return (item);
                }
            }
            return 0;
        }

        public int distanceFromWallMiddle(int number, int wallSize){

            int biggest = wallSize * wallSize;
            int difference = biggest - number;
            int distance = difference % (wallSize-1);

            return Math.Abs((wallSize-1)/2 - distance);
        }

        public int getDistanceFromMiddle(int number){
            int wallSize = getBigger(number);
            int dist = distanceFromWallMiddle(number, wallSize);
            return dist + (wallSize -1 )/2;
        }
    }
}
