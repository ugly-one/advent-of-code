using Common;
using day2;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace day2
{
    [TestFixture()]
    public class Part1
    {
        [Test()]
        public void TestCalculatingHash()
        {
            var testCases = new Dictionary<string, (int, int)>
            {
                ["ababab"] = (0, 1),
                ["abcdee"] = (1, 0),
                ["aabcdd"] = (1, 0),
                ["abcccd"] = (0, 1),
                ["abbcde"] = (1, 0),
                ["bababc"] = (1, 1),
                ["abcdef"] = (0, 0)
            };

            var solver = new Solver();
            int answer = solver.GetHash(testCases.Keys);

            Assert.AreEqual(12, answer);
        }

        [Test()]
        public void TestCalculatingOccurences()
        {
            var testCase = "ababab";

            var solver = new Solver();
            (bool, bool) answer = solver.GetOccurences(testCase);

            Assert.IsFalse(answer.Item1);
            Assert.IsTrue(answer.Item2);
        }

        [Test()]
        public void SolveReturnsSolutionWhenInputGiven()
        {
            var solver = new Solver();

            var input = FileReader.Read("../../day2/input.txt");
            var answer = solver.GetHash(input);
            Assert.AreEqual(5681, answer);
        }
    }
}
