using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace day1
{
    public class Solver2
    {
        private readonly IEnumerable<int> m_input;
        private HashSet<int> m_seenValues;
        int m_previousSum;

        public Solver2(IEnumerable<int> input)
        {
            m_input = input;
            m_seenValues = new HashSet<int>();
            m_previousSum = 0;
        }

        public int Run()
        {
            while (true)
            {
                foreach (var value in m_input)
                {
                    var newSum = m_previousSum + value;
                    if (m_seenValues.Contains(newSum))
                        return newSum;
                    m_seenValues.Add(newSum);
                    m_previousSum = newSum;
                }
            }
        }
    }
}
