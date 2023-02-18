using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day18;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace tests.day18
{
    [TestClass]
    public class MyProgramTests
    {
        private MyProgram assembler;

        [TestInitialize]
        public void init()
        {
            assembler = new MyProgram();
        }

        [TestMethod]
        public void SmallTest()
        {
            ReadDataFromFile("../../../day18/smallInput.txt");

            assembler.Run();

            Assert.AreEqual(4, assembler.LastRecoveredSoundValue);
        }


        [TestMethod]
        public void Test()
        {
            ReadDataFromFile("../../../day18/Input.txt");

            assembler.Run();

            Assert.AreEqual(1187, assembler.LastRecoveredSoundValue);
        }

        private void ReadDataFromFile(string argFileName)
        {
            using (var sr = new StreamReader(argFileName))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                    assembler.AddInstruction(line.TrimEnd());
            }
        }
    }
}
