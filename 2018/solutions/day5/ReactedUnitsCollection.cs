using System;
using System.Linq;

namespace Solutions.day5
{
    public class ReactedUnitsCollection
    {
        private int size;
        private bool[] units;
        private int currentIndex = 0;

        public ReactedUnitsCollection(int size)
        {
            this.size = size;
            units = new bool[size];
            AvailableCount = size;
        }

        public int AvailableCount { get; internal set; }

        public int[] GetAvailableIndexes()
        {
            int[] availableIndexes = new int[AvailableCount];
            var current = 0;
            for (int i = 0; i < size; i++)
            {
                if (!units[i]) availableIndexes[current] = i;
                current++;
            }

            return availableIndexes;
        }

        public int GetNextAvailable()
        {
            for (int i = currentIndex; i < size; i++)
            {
                if (!units[i])
                {
                    currentIndex = i + 1;
                    return i;
                }
            }

            throw new ArgumentOutOfRangeException();
        }

        public void React(int v1, int v2)
        {
            units[v1] = true;
            units[v2] = true;

            AvailableCount = units.Count(u => !u);
            currentIndex = 0;
        }
    }
}
