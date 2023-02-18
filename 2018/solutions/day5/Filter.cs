using System;
namespace Solutions.day5
{
    public class Filter
    {
        public Filter()
        {
        }

        /// <summary>
        /// Removes all occurences (lower and upper case) of the given letter
        /// </summary>
        /// <returns>The do.</returns>
        /// <param name="toFilterOut">lower or upper case of a letter to filter out</param>
        public static string Do(char toFilterOut, string input)
        {
            char[] result = new char[input.Length];
            var resultLength = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i].ToString().ToLower() != toFilterOut.ToString().ToLower())
                {
                    result[resultLength] = input[i];
                    resultLength++;
                }
            }

            return new string(result,0 , resultLength);
        }
    }
}
