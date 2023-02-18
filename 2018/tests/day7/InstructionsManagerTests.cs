using Common;
using NUnit.Framework;
using Solutions.day7;
using System;
using System.Linq;

namespace day7
{
    [TestFixture()]
    public class InstructionsManagerTests
    {
        private Instruction[] instructions;

        [SetUp]
        public void Init()
        {
            instructions = new Instruction[]
            {
                new Instruction('C', 'A'),
                new Instruction('C', 'F'),
                new Instruction('A', 'B'),
                new Instruction('A', 'D'),
                new Instruction('B', 'E'),
                new Instruction('D', 'E'),
                new Instruction('F', 'E'),
            };
        }

        [Test()]
        public void CreateSteps_ReturnsCAsTheOnlyStepWhichIsNotBlocked()
        {
            var steps = instructions.CreateSteps();

            Assert.AreEqual('C', steps.FirstOrDefault(s => !s.IsBlocked).Value);
        }

        [Test()]
        public void Order()
        {
            var steps = instructions.CreateSteps();
            var manager = new InstructionsManager(new Worker(0));
            var orderedSteps = manager.Order(steps, out var elapsedTime);

            string result = "";
            foreach (var step in orderedSteps)
            {
                result += step.Value;
            }
            Assert.AreEqual("CABDFE", result);
        }

        [Test()]
        public void Part1Solution()
        {
            instructions = FileReader.Read("../../day7/input.txt").Select(i => i.ToInstruction()).ToArray();
            var steps = instructions.CreateSteps();
            var manager = new InstructionsManager(new Worker(0));
            var orderedSteps = manager.Order(steps, out var elapsedTime);

            string result = "";
            foreach (var step in orderedSteps)
            {
                result += step.Value;
            }
            Assert.AreEqual("CFGHAEMNBPRDISVWQUZJYTKLOX", result);
        }

        [Test()]
        public void Part2Order()
        {
            var steps = instructions.CreateSteps();
            var manager = new InstructionsManager( new Worker[]{
                new Worker(0),
                new Worker(0) });
            var orderedSteps = manager.Order(steps, out var elapsedTime);

            string result = "";
            foreach (var step in orderedSteps)
            {
                result += step.Value;
            }
            Assert.AreEqual("CABFDE", result);
            Assert.AreEqual(15, elapsedTime);
        }

        [Test()]
        public void Part2Solution()
        {
            instructions = FileReader.Read("../../day7/input.txt").Select(i => i.ToInstruction()).ToArray();
            var steps = instructions.CreateSteps();
            var manager = new InstructionsManager(new Worker[]{
                new Worker(60),
                new Worker(60),
                new Worker(60),
                new Worker(60),
                new Worker(60) });
            var orderedSteps = manager.Order(steps, out var elapsedTime);
            Assert.AreEqual(828, elapsedTime);
        }
    }
}
