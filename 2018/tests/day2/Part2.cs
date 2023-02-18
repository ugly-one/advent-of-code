using Common;
using day2;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace day2
{
    [TestFixture()]
    public class Part2
    {
        private string[] testData = new string[]
        {
            "abcde",
            "fghij",
            "klmno",
            "pqrst",
            "fguij",
            "axcye",
            "wvxyz",
        };

        private Solver2 solver;

        [SetUp]
        public void Initialize()
        {
            solver = new Solver2();
        }

        [Test()]
        public void SolveReturnsCommonPartFromStringCollection()
        {
            var result = solver.Solve(testData);

            Assert.AreEqual("fgij", result);
        }

        [Test()]
        public void CompareReturnsTrueWhen2StringsAreDifferentByExatlyOneCharacter()
        {
            var result = solver.Compare("blabla", "blabra", out var common);
            Assert.IsTrue(result);
            Assert.AreEqual(common, "blaba");
        }

        [Test()]
        public void CompareReturnsFalseWhen2StringsAreDifferentBy2Characters()
        {
            var result = solver.Compare("blabla", "brabra", out var common);
            Assert.IsFalse(result);
        }

        [Test()]
        public void SolveReturnsSolutionWhenInputGiven()
        {
            var data = FileReader.Read("../../day2/input.txt");
            var result = solver.Solve(data.ToArray());
            Assert.AreEqual("uqyoeizfvmbistpkgnocjtwld", result);
        }
    }
}
