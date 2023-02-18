using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace solutions.day18
{
    public class MainProcess
    {
        public IList<ProgramWithQueue> Programs;
        //public ManualResetEvent AllProgramsDone = new ManualResetEvent(false);

        public MainProcess(int argAmountOfPrograms)
        {
            Programs = new List<ProgramWithQueue>(argAmountOfPrograms);
            for (int i = 0; i < argAmountOfPrograms; i++)
            {
                var queue = new Queue();
                var program = new ProgramWithQueue(queue, i);
                Programs.Add(program);
           //     program.ProgramDone += OnProgramDone;
            }

            foreach (var program in Programs)
            {
                // find all other programs
                var otherPrograms = Programs.Where(p => p.ID != program.ID);
                foreach (var otherProgram in otherPrograms)
                {
                    program.MessageSend += otherProgram.m_queue.OnMessageSend;
                }
            }
        }

        public void AddInstructions(ProgramWithQueue argProgram, IEnumerable<string> argInstructions)
        {
            foreach (var instr in argInstructions)
            {
                argProgram.AddInstruction(instr);
            }
        }

        public void Run()
        {
            foreach (var item in Programs)
            {
                item.Run();
            }

            while (true)
            {
                Thread.Sleep(2000);
                if (AreAllProgramsDone()) break;
                if (AreInDeadLock()) break;
            }
        }

        private bool AreInDeadLock()
        {
            return Programs.Where(program => program.IsWaiting).Count() == Programs.Count;
        }

        private bool AreAllProgramsDone()
        {
            return Programs.Where(program => program.IsProgramDone).Count() == Programs.Count;
        }

        //private void OnProgramDone(object sender, EventArgs e)
        //{
        //    if (Programs.Where(program => program.IsProgramDone).Count() == Programs.Count)
        //        AllProgramsDone.Set();
        //}
    }
}
