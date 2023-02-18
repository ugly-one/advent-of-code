using System;
using System.Text.RegularExpressions;

namespace Solutions.day7
{
    public static class StringToInstructionParser
    {
        public static Instruction ToInstruction(this string input)
        {
            var regex = new Regex(@"\AStep (?<part1>\D) must be finished before step (?<part2>\D)");
            var matches = regex.Matches(input);

            var part1 = matches[0].Groups["part1"].Value;
            var part2 = matches[0].Groups["part2"].Value;

            return new Instruction(part1[0], part2[0]);
        }
    }
}
