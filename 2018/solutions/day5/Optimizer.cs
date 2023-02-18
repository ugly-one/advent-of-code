using System;
using System.Linq;

namespace Solutions.day5
{
    public class Optimizer
    {
        public Optimizer()
        {
        }

        /// <summary>
        /// Returns an array of distinct character in the given <paramref name="input"/>. 
        /// Characters are case insensite, so 'c' and 'C' are considered equivalent.
        /// </summary>
        /// <returns>an array of distinct characters</returns>
        /// <param name="input">string to search in</param>
        public static char[] GetDistinct(string input)
        {
            return input.ToLower().Distinct().ToArray();
        }

        /// <summary>
        /// Tries to remove one unit (fx. 'a' and 'A' at the time, and performs all reaction on it) 
        /// and finds the shortest polymer found that way
        /// </summary>
        /// <returns>the lenght of the shortest polymer produced that way</returns>
        public static int GetOptimalPolymer(string input)
        {

            char[] characters = GetDistinct(input);
            int minimalLength = input.Length;
            foreach (var character in characters)
            {
                var filteredPolymer = Filter.Do(character, input);
                //var polymerAfterReaction = Polymer.PerformAllReactions(filteredPolymer);
                var reacted = true;
                var polymer = new Polymer(filteredPolymer);
                while (reacted)
                {
                    reacted = polymer.React();
                }
                var length = polymer.GetActiveCount();
                if (length < minimalLength)
                    minimalLength = length; 
            }

            return minimalLength;
        }
    }
}
