using System;
using System.Collections.Generic;
using System.Text;

namespace solutions.day15
{
    public class Generator
    {
        private ulong previousValue;
        private uint factor;

        public Generator(uint argStartingValue, uint argFactor)
        {
            this.previousValue = argStartingValue;
            this.factor = argFactor;
        }

        public virtual ulong Generate()
        {
            ulong newValue = (previousValue * factor) % 2147483647;

            previousValue = newValue;
            return newValue;
        }
    }
}
