using System;
using System.Linq;

namespace Solutions.day5
{
    public class Polymer
    {

        private string m_value;
        private ReactedUnitsCollection reactedUnitsCollection;

        public Polymer(string value)
        {
            m_value = value;
            reactedUnitsCollection = new ReactedUnitsCollection(value.Length);
        }

        public int GetActiveCount()
        {
            return reactedUnitsCollection.AvailableCount;
        }

        /// <summary>
        /// </summary>
        public bool React()
        {
            try
            {
                var firstAvailable = reactedUnitsCollection.GetNextAvailable();
                var secondAvailable = reactedUnitsCollection.GetNextAvailable();

                while (true)
                {
                    if (CausesReaction(m_value[firstAvailable], m_value[secondAvailable]))
                    {
                        reactedUnitsCollection.React(firstAvailable, secondAvailable);
                        return true;
                    }
                    firstAvailable = secondAvailable;
                    secondAvailable = reactedUnitsCollection.GetNextAvailable();
                }

            }
            catch
            {
                return false;
            }

        }


        /// <summary>
        /// returns true if 2 characters are the same - case insensitive
        /// </summary>
        /// <returns><c>true</c>, if reaction was causesed, <c>false</c> otherwise.</returns>
        /// <param name="v1">V1.</param>
        /// <param name="v2">V2.</param>
        private static bool CausesReaction(char v1, char v2)
        {
            if (v1 == v2) return false;
            if (char.ToLower(v1) == char.ToLower(v2)) return true;
            return false;
        }
    }
}
