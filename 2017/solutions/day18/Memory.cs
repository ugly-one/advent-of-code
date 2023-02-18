using System;
using System.Collections.Generic;
using System.Linq;

namespace solutions.day18
{
    public class Memory
    {
        public List<Register> Registers; 

        public Memory()
        {
            Registers = new List<Register>();
        }

        private long ParseValue(string argValue)
        {
            long value;
            if (!long.TryParse(argValue, out value))
            {
                value = ReadValue(argValue[0]);
            }

            return value;
        }

        public void Set(char argName, string argValue)
        {
            PerformOperation(argName, (v1) => ParseValue(argValue));
        }

        public void Add(char argName, string argValue)
        {
            PerformOperation(argName, (v1) => v1 + ParseValue(argValue));
        }

        public void Multiply(char argName, string argValue)
        {
            PerformOperation(argName, (v1) => v1 * ParseValue(argValue));
        }

        public void Mod(char argName, string argValue)
        {
            PerformOperation(argName, (v1) => v1 % ParseValue(argValue));
        }

        public long ReadValue(char argName)
        {
            var register = FindOrCreate(argName);
            return register.Value;
        }

        private void PerformOperation(char argName, Func<long, long> operation)
        {
            var register = FindOrCreate(argName);
            register.SetValue(operation(register.Value));
        }

        private Register FindOrCreate(char argName)
        {
            var register = Registers.Where(r => r.Name == argName).FirstOrDefault();

            if (register == null)
            {
                register = new Register(argName);
                Registers.Add(register);
            }

            return register;
        }
    }
}
