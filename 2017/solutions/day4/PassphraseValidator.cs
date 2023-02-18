using System;
using System.Collections.Generic;
using System.Linq;

namespace solutions.day4
{
    public class PassphraseValidator
    {
        public bool Validate(string input)
        {
            bool result = true;
            string[] words = input.Split(' ');
            IEnumerable<string> usefulWords = words.Skip(1);
            foreach (string word in words)
            {
                if (usefulWords.ContainsAnagram(word)) return false;
                usefulWords = usefulWords.Skip(1);
            }

            return result;
        }
    }
}
