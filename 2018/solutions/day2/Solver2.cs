using System;
using System.Collections.Generic;
using System.Linq;

namespace day2
{
    public class Solver2
    {
        public string Solve(string[] data)
        {
            if (data.Length == 0)
                throw new Exception("string not found");

            var dataCount = data.Count();
            for (int i = 0; i < dataCount; i++)
            {
                for (int j = i+1; j < dataCount; j++)
                {
                    if (Compare(data[i], data[j], out string common))
                        return common;
                }
            }

            throw new Exception("string not found");
        }

        public bool Compare(string s1, string s2, out string common)
        {
            common = "";
            var length = s1.Length;
            bool differenceFound = false;
            if (length != s2.Length) return false;
            for (int i = 0; i < length; i++)
            {
                if (s1[i] != s2[i])
                {
                    if (differenceFound) return false; // difference found twice
                    differenceFound = true;
                }
                else
                {
                    common = common + s1[i];
                }
            }
            return differenceFound;
        }
    }
}
