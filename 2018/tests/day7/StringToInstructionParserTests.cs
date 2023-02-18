using Common;
using NUnit.Framework;
using Solutions.day7;
using System;
using System.Linq;

namespace day7
{
    [TestFixture()]
    public class StringToInstructionParserTests
    {
        [Test()]
        public void TestCase()
        {
            var input = "Step B must be finished before step E can begin.";

            Assert.AreEqual('B', input.ToInstruction().Part1);
            Assert.AreEqual('E', input.ToInstruction().Part2);
        }
    }
}
