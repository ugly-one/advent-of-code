using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day5;

namespace tests.day5
{
    [TestClass]
    public class UnitTest1
    {
        private Class1 c;
        [TestInitialize]
        public void init(){
            c = new Class1(new int[] {0,3,0,1,-3});                
        }
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(5, c.instructions.Length);
        }

        [TestMethod]
        public void getInstructions_Populates4rdElementWith1()
        {
            Assert.AreEqual(1, c.instructions[3]);
        }

        [TestMethod]
        public void increase_IncreasesByOne(){
            var currentValue = c.instructions[0];
            c.increaseCurrentPosition();
            Assert.AreEqual(currentValue+1, c.instructions[0]);
        }

        [TestMethod]
        public void startJumping_returns10(){
            var steps = c.startJumping();
            Assert.AreEqual(10, steps);
        }

        [TestMethod]
        public void Real_startJumping_returns24490906(){
            var c2 = new Class1(loadInstructionsFromInput());
            var steps = c2.startJumping();
            Assert.AreEqual(24490906, steps);
        }


        private IEnumerable<int> loadInstructionsFromInput()
        {
            var result = new List<int>();
            int number;
            using (var stream = new StreamReader("../../../day5/input.txt"))
            {
                string line = "";
                while ((line = stream.ReadLine()) != null)
                {
                    Int32.TryParse(line, out number);
                    result.Add(number);
                }
            }
            return result.ToArray();
        }
    }
}
