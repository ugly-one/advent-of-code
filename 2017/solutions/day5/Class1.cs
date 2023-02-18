using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace solutions.day5
{
    public class Class1
    {
        public int[] instructions;
        private int currentPosition;

        public Class1(IEnumerable<int> instructions)
        {
            currentPosition = 0;
            this.instructions = instructions.ToArray();
        }

        public void jump(int jumpSize)
        {
            if (jumpSize >= 3) decreateCurrentPosition();
            else increaseCurrentPosition();
            currentPosition += jumpSize;
        }

        public void increaseCurrentPosition()
        {
            instructions[currentPosition]++;
        }

        public void decreateCurrentPosition(){
            instructions[currentPosition]--;
        }
        public int startJumping()
        {
            int steps = 0;
            while (currentPosition < instructions.Length)
            {
                jump(instructions[currentPosition]);
                steps++;
            }
            return steps;
        }
    }
}
