using System;
using System.Collections.Generic;
using System.Text;

namespace solutions.day15
{
    public class UpgradedGenerator : Generator
    {
        private ulong criteria;

        public UpgradedGenerator(uint argStartingValue, uint argFactor, int argCriteria) : base(argStartingValue, argFactor)
        {
            criteria = (ulong)argCriteria;
        }

        public override ulong Generate()
        {
            ulong nextValue;
            do
            {
                nextValue = base.Generate();
            } while (nextValue % criteria != 0);
            
            return nextValue;
        }
    }
}
