using System;
using System.Diagnostics;

namespace solutions.day18
{
    internal class Instruction
    {
        private Action action;
        public Instruction(Action argAction)
        {
            action = argAction;
        }

        public void Execute()
        {
            Debug.WriteLine($"executing { action.Method.Name.ToString()}");
            action();
        }
    }
}