using System;
using System.Collections.Generic;
using System.Text;

namespace solutions.day18
{
    public class MyProgram
    {
        internal Memory memory;
        private List<Action> instructions;
        private long lastSoundValue;
        public long LastRecoveredSoundValue;
        private long currentPosition;
        public MyProgram()
        {
            memory = new Memory();
            instructions = new List<Action>();
            lastSoundValue = 0;
            LastRecoveredSoundValue = 0;
            currentPosition = 0;
        }

        public void AddInstruction(string instruction)
        {
            string[] instructionParts = instruction.Split(' ');
            string instructionName = instructionParts[0];
            char registerName = instructionParts[1][0];
            string value;

            switch (instructionName)
            {
                case "set":
                    value = instructionParts[2];
                    instructions.Add(CreateAction(() => memory.Set(registerName, value)));
                    break;
                case "add":
                    value = instructionParts[2];
                    instructions.Add(CreateAction(() => memory.Add(registerName, value)));
                    break;
                case "mul":
                    value = instructionParts[2];
                    instructions.Add(CreateAction(() => memory.Multiply(registerName, value)));
                    break;
                case "mod":
                    value = instructionParts[2];
                    instructions.Add(CreateAction(() => memory.Mod(registerName, value)));
                    break;
                case "snd":
                    instructions.Add(CreateAction(() => Send(registerName)));
                    break;
                case "rcv":
                    instructions.Add(CreateAction(() => Receive(registerName)));
                    break;
                case "jgz":
                        value = instructionParts[2];
                        instructions.Add(() => Jump(registerName, value));
                    break;
                default:
                    break;
            }
        }

        private Action CreateAction(Action argAction)
        {
            return () =>
            {
                argAction();
                currentPosition++;
            };
        }

        private void Jump(char argName, string argValue)
        {
            long valueToCompare;
            if (!long.TryParse(argName.ToString(), out valueToCompare))
                valueToCompare = memory.ReadValue(argName); 

            if (valueToCompare > 0)
            {
                if (long.TryParse(argValue, out long value))
                    currentPosition += value;
                else
                    currentPosition += memory.ReadValue(argValue[0]);
            }
            else
            {
                currentPosition++;
            }
        }

        internal virtual void Receive(char argName){
            if (memory.ReadValue(argName) != 0)
            {
                LastRecoveredSoundValue = lastSoundValue;
            }
        }

        protected virtual void Send(char argName)
        {
            lastSoundValue = memory.ReadValue(argName);
        }

        public virtual void Run()
        {
            Action instruction = instructions[0];
            do
            {
                Console.WriteLine($"processing instruction {currentPosition} {instruction.Method.Name}");
                instruction();
                if (LastRecoveredSoundValue != 0) break;
                if (currentPosition < 0) break;
                if (currentPosition >= instructions.Count) break;
                
                instruction = instructions[(int)currentPosition];

            } while (true);
        }
    }
}
