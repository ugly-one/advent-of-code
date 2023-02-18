using System.Collections.Generic;
using System.Linq;

namespace solutions.day17
{
    public class Spinner
    {
        private int m_currentPosition;
        private List<int> m_items;

        public int CurrentPosition {
            get
            {
                return m_currentPosition;
            }
        }

        public Spinner()
        {
            m_currentPosition = 0;
            m_items = new List<int>() { 0 };
        }

        public IEnumerable<int> Spin(int times, int argSteps)
        {
            foreach (var index in IndexGenerator.Generate(argSteps).Take(times))
            {
                m_currentPosition = index;
                m_items.Insert(m_currentPosition, m_items.Count);
            }
            return m_items.ToArray();
        }

        public int SpinAndGetAfterZero(int times, int argSteps)
        {
            int valueAtIndexOne = 0;
            int counter = 0;
            foreach (var index in IndexGenerator.Generate(argSteps).Take(times))
            {
                counter++;
                if (index == 1) valueAtIndexOne = counter;
            }

            return valueAtIndexOne;
        }
    }

}
