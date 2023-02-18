using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day18;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace tests.day18
{
    [TestClass]
    public class ProgramWithQueueTests
    {
        private MainProcess process;

        [TestInitialize]
        public void init()
        {
        }

        [TestMethod]
        public void Program_receivesMessages()
        {
            process = new MainProcess(1);

            process.AddInstructions(process.Programs[0], new[] { "rcv a" });

            process.Programs[0].m_queue.Add(2);

            process.Run();

            //process.AllProgramsDone.WaitOne();

            Assert.AreEqual(process.Programs[0].ReadValue('a'), 2);
        }

        [TestMethod]
        public void TwoProcesses_BothReceivesValuesFromEathOther()
        {
            process = new MainProcess(2);

            var instructions = new[]
            {
                "rcv a",
                "add b 89",
                "add c 3",
                "snd b"
            };
            process.AddInstructions(process.Programs[0],instructions);

            instructions = new[]
            {
                "add r 4",
                "snd r",
                "rcv r",
            };

            process.AddInstructions(process.Programs[1],instructions);
            process.Run();
            //process.AllProgramsDone.WaitOne();

            Assert.AreEqual(4, process.Programs[0].ReadValue('a'));
            Assert.AreEqual(89, process.Programs[1].ReadValue('r'));
        }

        [TestMethod]
        public void Process_SameInstructions()
        {
            process = new MainProcess(2);
            var inst = new[]
            {
                "snd 1",
                "snd 2",
                "snd p",
                "rcv a",
                "rcv b",
                "rcv c",
            };
            process.AddInstructions(process.Programs[0], inst);
            process.AddInstructions(process.Programs[1], inst);

            process.Run();
            //process.AllProgramsDone.WaitOne();

            Assert.AreEqual(1, process.Programs[0].ReadValue('a'), "program 0 - a");
            Assert.AreEqual(1, process.Programs[1].ReadValue('a'), "program 1 - a");
            Assert.AreEqual(2, process.Programs[0].ReadValue('b'), "program 0 - b");
            Assert.AreEqual(2, process.Programs[1].ReadValue('b'), "program 1 - b");
            Assert.AreEqual(1, process.Programs[0].ReadValue('c'), "program 0 - c");
            Assert.AreEqual(0, process.Programs[1].ReadValue('c'), "program 1 - c");
        }

        [TestMethod]
        public void Process_SameInstructions_DeadLock()
        {
            process = new MainProcess(2);
            var inst = new[]
            {
                "snd 1",
                "snd 2",
                "snd p",
                "rcv a",
                "rcv b",
                "rcv c",
                "rcv d",
            };
            process.AddInstructions(process.Programs[0], inst);
            process.AddInstructions(process.Programs[1], inst);

            process.Run();
            //process.AllProgramsDone.WaitOne();

            Assert.AreEqual(1, process.Programs[0].ReadValue('a'), "program 0 - a");
            Assert.AreEqual(1, process.Programs[1].ReadValue('a'), "program 1 - a");
            Assert.AreEqual(2, process.Programs[0].ReadValue('b'), "program 0 - b");
            Assert.AreEqual(2, process.Programs[1].ReadValue('b'), "program 1 - b");
            Assert.AreEqual(1, process.Programs[0].ReadValue('c'), "program 0 - c");
            Assert.AreEqual(0, process.Programs[1].ReadValue('c'), "program 1 - c");
        }

        [TestMethod]
        public void Process_SendValue()
        {
            process = new MainProcess(2);
            var inst = new[]
            {
                "snd 1",
                "snd 2",
                "snd p",
                "rcv a",
                "rcv b",
                "rcv c",
                "rcv d",
            };
            process.AddInstructions(process.Programs[0], inst);
            process.AddInstructions(process.Programs[1], inst);

            process.Run();

            Assert.AreEqual(3, process.Programs[1].SendCounter);
        }

        [TestMethod]
        public void Process_MainChallange()
        {
            process = new MainProcess(2);
            var inst = ReadDataFromFile("../../../day18/Input.txt");
            process.AddInstructions(process.Programs[0], inst);
            process.AddInstructions(process.Programs[1], inst);

            process.Run();

            Assert.AreEqual(5969, process.Programs[1].SendCounter);
        }

        private IEnumerable<string> ReadDataFromFile(string argFileName)
        {
            var lines = new List<string>();
            using (var sr = new StreamReader(argFileName))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                    lines.Add(line.TrimEnd());
            }
            return lines;
        }
    }
    
}
