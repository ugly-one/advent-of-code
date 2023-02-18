using System;
using System.Collections.Generic;

namespace day2
{
    public class Solver
    {
        public Solver()
        {
        }

        public (bool, bool) GetOccurences(string testCase)
        {
            var dic = new Dictionary<char, int>();

            foreach (var letter in testCase)
            {
                if (dic.ContainsKey(letter))
                    dic[letter]++;

                else
                    dic.Add(letter, 1);
            }

            var wasTwice = false;
            var wasThreeTimes = false;
            foreach (var keyValuePair in dic)
            {
                if (!wasTwice && keyValuePair.Value == 2) wasTwice = true;
                if (!wasThreeTimes && keyValuePair.Value == 3) wasThreeTimes = true;
            }

            return (wasTwice, wasThreeTimes);
        }

        public int GetHash(IEnumerable<string> ids)
        {
            int twiceCounter = 0;
            int threeTimesCounter = 0;

            foreach (var item in ids)
            {
                var occurences = GetOccurences(item);
                if (occurences.Item1) twiceCounter++;
                if (occurences.Item2) threeTimesCounter++;
            }

            return twiceCounter * threeTimesCounter;
        }
    }
}
