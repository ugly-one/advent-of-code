using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day9;

namespace tests.day9
{
    [TestClass]
    public class GarbageCountTests
    {
        private StreamProcessor _processor;

        [TestInitialize]
        public void init(){
            _processor = new StreamProcessor();
        }

        [TestMethod]
        public void case1(){
            TestString("{}", 0);
        }

        [TestMethod]
        public void case2(){
            TestString("<random characters>", 17);
        }

        [TestMethod]
        public void case3(){
            TestString("<<<<>", 3);
        }

        [TestMethod]
        public void case4(){
            TestString("<{!>}>", 2);
        }

        [TestMethod]
        public void case5(){
            TestString("<!!>", 0);
        }
        [TestMethod]
        public void case6(){
            TestString("<!!!>>", 0);
        }
        [TestMethod]
        public void case7(){
            TestString("<{o'i!a,<{i<a>", 10);
        }

        [TestMethod]
        public void StreamProcessor_returnsMainGarbageCount()
        {
            string input = File.ReadAllText("../../../day9/input");
            TestString(input, 6285);
        }
        private void TestString(string input, int expectedGarbageAmount){
            var result = _processor.Process(input);
            Assert.AreEqual(expectedGarbageAmount, result.garbageCount);
        }
    }
}
