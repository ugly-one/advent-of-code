using System;
using System.Collections.Generic;
using System.Text;

namespace solutions.day15
{
    public class Judge
    {
        private Generator genA;
        private Generator genB;

        public Judge(Generator argGenA, Generator argGenB)
        {
            genA = argGenA;
            genB = argGenB;
        }

        public uint GetCount(uint argRounds)
        {
            uint count = 0;
            ulong valueA;
            ulong valueB;
            for (int i = 0; i < argRounds; i++)
            {
                valueA = genA.Generate();
                valueB = genB.Generate();
                if (GetLowest16Bits(valueA) == GetLowest16Bits(valueB)) count++;
            }

            return count;
        }

        private string GetLowest16Bits(ulong value)
        {
            value = value & 0xffff;
            return Convert.ToString((long)value, 2);
        }
    }
}
