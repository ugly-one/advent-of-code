using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day9;

namespace tests.day9
{
    [TestClass]
    public class ScoreTests
    {
        private StreamProcessor _processor;

        [TestInitialize]
        public void init(){
            _processor = new StreamProcessor();
        }

        [TestMethod]
        public void StreamProcessor_returnsScore1()
        {
            TestString("{}", 1);
        }

        [TestMethod]
        public void StreamProcessor_returnsScore6()
        {
            TestString("{{{}}}", 6);
        }
        [TestMethod]
        public void StreamProcessor_returnsScore5()
        {
            TestString("{{},{}}", 5);
        }

        [TestMethod]
        public void StreamProcessor_returnsScore16()
        {
            TestString("{{{},{},{{}}}}", 16);
        }
        [TestMethod]
        public void StreamProcessor_returnsScore1_2()
        {
            TestString("{<a>,<a>,<a>,<a>}", 1);
        }

        [TestMethod]
        public void StreamProcessor_returnsScore9()
        {
            TestString("{{<ab>},{<ab>},{<ab>},{<ab>}}", 9);
        }
        [TestMethod]
        public void StreamProcessor_returnsScore9_2()
        {
            TestString("{{<!!>},{<!!>},{<!!>},{<!!>}}", 9);
        }
        [TestMethod]
        public void StreamProcessor_returnsScore3()
        {
            TestString("{{<a!>},{<a!>},{<a!>},{<ab>}}", 3);
        }

        [TestMethod]
        public void StreamProcessor_returnsMainScore()
        {
            string input = File.ReadAllText("../../../day9/input");
            TestString(input, 11846);
        }

        private void TestString(string input, int expectedScore){
            var result = _processor.Process(input);
            var totalScore = result.Groups.Select(g => g.Score).Sum();
            Assert.AreEqual(expectedScore, totalScore);
        }
    }
}
