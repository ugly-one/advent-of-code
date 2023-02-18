using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using Solutions.day5;
using Common;

namespace day5
{
    [TestFixture()]
    public class OptimizerTests
    {
        [Test()]
        public void GetDistinctTest()
        {
            var input = "dabAcCaCBAcCcaDA";
            char[] unitsToTest = Optimizer.GetDistinct(input);

            var expected = new char[]
            {
                'd',
                'a',
                'b',
                'c'
            };
            Assert.IsTrue(AreEqual(expected, unitsToTest));
        }

        [Test()]
        public void GetOptimalTest()
        {
            var input = "dabAcCaCBAcCcaDA";
            int expectedResult = 4;

            var result = Optimizer.GetOptimalPolymer(input);

            Assert.AreEqual(expectedResult, result);
        }

        [Test()]
        public void GetOptimalTestOnInput()
        {
            var input = FileReader.Read("../../day5/input.txt").FirstOrDefault();
            int expectedResult = 6918;

            var result = Optimizer.GetOptimalPolymer(input);

            Assert.AreEqual(expectedResult, result);
        }

        private bool AreEqual(char[] c1, char[] c2)
        {
            if (c1.Length != c2.Length) return false;
            for (int i = 0; i < c1.Length; i++)
            {
                if (c1[i] != c2[i]) return false;
            }
            return true;
        }
    }
}
