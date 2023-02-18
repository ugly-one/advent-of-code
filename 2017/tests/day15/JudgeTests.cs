using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day15;

namespace tests.day15
{
    [TestClass]
    public class JudgeTests
    {
        private Judge judge;

        [TestInitialize]
        public void Init()
        {
            judge = new Judge(new Generator(65, 16807), new Generator(8921, 48271));
        }

        [TestMethod]
        public void GetCount_Returns1_After5Rounds()
        {
            uint result = judge.GetCount(5u);
            Assert.AreEqual(1u, result);
        }

        [TestMethod]
        public void GetCount_Returns588_After40MillionRounds()
        {
            uint result = judge.GetCount(40000000u);
            Assert.AreEqual(588u, result);
        }

        [TestMethod]
        public void GetCount_Returns638_After40MillionRounds_RealData()
        {
            judge = new Judge(new Generator(289, 16807), new Generator(629, 48271));
            uint result = judge.GetCount(40000000u);
            Assert.AreEqual(638u, result);
        }

        [TestMethod]
        public void GetCount_Returns1_After1056_WhenUpgraded()
        {
            judge = new Judge(new UpgradedGenerator(65, 16807, 4), new UpgradedGenerator(8921, 48271, 8));
            uint result = judge.GetCount(1056u);
            Assert.AreEqual(1u, result);
        }

        [TestMethod]
        public void GetCount_Returns343_After5Million_WhenUpgraded_RealData()
        {
            judge = new Judge(new UpgradedGenerator(289, 16807, 4), new UpgradedGenerator(629, 48271, 8));
            uint result = judge.GetCount(5000000u);
            Assert.AreEqual(343u, result);
        }
    }
}
