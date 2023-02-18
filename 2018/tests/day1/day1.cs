using Common;
using day1;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace day1
{
    [TestFixture()]
    public class Day1
    {
        [Test()]
        public void Part1Test()
        {
            IEnumerable<string> lines = FileReader.Read("../../../day1/input.txt");
            IEnumerable<int> numbers = lines.Select(n => int.Parse(n));

            var solver = new Solver();
            Assert.AreEqual(508, solver.Run(numbers));
        }

        [Test()]
        public void Part2Test()
        {
            IEnumerable<string> lines = FileReader.Read("../../../day1/input.txt");
            IEnumerable<int> numbers = lines.Select(n => int.Parse(n));

            var solver = new Solver2(numbers);
            Assert.AreEqual(549, solver.Run());

        }
    }
}
