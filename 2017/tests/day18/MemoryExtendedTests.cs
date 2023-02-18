using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day18;
using System;
using System.Collections.Generic;
using System.Text;

namespace tests.day18
{
    [TestClass]
    public class MemoryExtendedTests
    {
        private Memory memory;

        [TestInitialize]
        public void Init()
        {
            memory = new Memory();
        }

        [TestMethod]
        public void Set_setsValueCorrectly_WhenAnotherRegisterGivenAsValue()
        {
            memory.Set('a', "2");
            memory.Set('b', "a");

            Assert.AreEqual(memory.ReadValue('b'), 2);
        }

        [TestMethod]
        public void Add()
        {
            memory.Set('a', "2");
            memory.Set('b', "4");
            memory.Add('b', "a");

            Assert.AreEqual(memory.ReadValue('b'), 6);
        }

        [TestMethod]
        public void Mul()
        {
            memory.Multiply('b', "17");

            Assert.AreEqual(memory.ReadValue('b'), 0);
        }
    }
}
