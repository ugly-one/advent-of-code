using System.Collections.Generic;
using System.Linq;

namespace Solutions.day7
{
    public static class InstructionsExtensions
    {
        public static IEnumerable<Step> CreateSteps(this IEnumerable<Instruction> instructions)
        {
            var steps = new Dictionary<char, Step>();
            foreach (var instruction in instructions)
            {
                if (!steps.TryGetValue(instruction.Part2, out Step step))
                {
                    step = new Step(instruction.Part2);
                    steps.Add(instruction.Part2, step);
                }
                step.SetIsBlocked();

                if (!steps.TryGetValue(instruction.Part1, out Step secondStep))
                {
                    secondStep = new Step(instruction.Part1);
                    steps.Add(instruction.Part1, secondStep);
                }
                secondStep.AddStepAfter(step);

                step.AddRequiredStep(secondStep);
            }
            return steps.Select(kv => kv.Value);
        }
    }
}
