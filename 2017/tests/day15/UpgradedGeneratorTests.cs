using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day15;

namespace tests.day15
{
    [TestClass]
    public class UpgradedGeneratorTests
    {
        private UpgradedGenerator generator;

        [TestInitialize]
        public void Init()
        {
            generator = new UpgradedGenerator(65, 16807, 4);
        }

        [TestMethod]
        public void GeneratorA_FirstValue()
        {
            Assert.AreEqual(1352636452ul, generator.Generate());
        }

        [TestMethod]
        public void GeneratorA_SecondValue()
        {
            TestHelper.GenerateMultipleTimes(generator, 1);
            Assert.AreEqual(1992081072ul, generator.Generate());
        }
    }
}
