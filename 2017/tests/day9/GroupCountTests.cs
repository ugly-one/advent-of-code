using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day9;

namespace tests.day9
{
    [TestClass]
    public class GroupCountTests
    {
        private StreamProcessor _processor;

        [TestInitialize]
        public void init(){
            _processor = new StreamProcessor();
        }

        [TestMethod]
        public void StreamProcessor_OnCleanInput_returns1Group()
        {
            TestString("{}", 1);
        }

        [TestMethod]
        public void StreamProcessor_returns3Group()
        {
            TestString("{{{}}}", 3);
        }

        [TestMethod]
        public void StreamProcessor_returns3Group_2()
        {
            TestString("{{},{}}", 3);
        }

        [TestMethod]
        public void StreamProcessor_returns6Group_()
        {
            TestString("{{{},{},{{}}}}", 6);
        }

        [TestMethod]
        public void StreamProcessor_SkipsGroupsInOneTrash()
        {
            TestString("{<{},{},{{}}>}", 1);
        }

        [TestMethod]
        public void StreamProcessor_SkipsGroupsInMultipleTrashes()
        {
            TestString("{<a>,<a>,<a>,<a>}", 1);
        }

        [TestMethod]
        public void StreamProcessor_SkipsOnlyTrash()
        {
            TestString("{{<a>},{<a>},{<a>},{<a>}}", 5);
        }

        [TestMethod]
        public void StreamProcessor_EndOfTrashCancelled()
        {
            TestString("{{<!>},{<!>},{<!>},{<a>}}", 2);
        }

        private void TestString(string input, int expectedAmountOfGroups){
            var result = _processor.Process(input);
            Assert.AreEqual(expectedAmountOfGroups, result.Groups.Count());
        }
    }
}
