using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day15;

namespace tests.day15
{
    [TestClass]
    public class GeneratorATests
    {
        private Generator generatorA;

        [TestInitialize]
        public void Init()
        {
            generatorA = new Generator(65, 16807);
        }

        [TestMethod]
        public void GeneratorA_generatesFirstValue_1092455()
        {
            Assert.AreEqual(1092455ul, generatorA.Generate());
        }

        [TestMethod]
        public void GeneratorA_generatesSecondValue_1181022009()
        {
            TestHelper.GenerateMultipleTimes(generatorA, 1);
            Assert.AreEqual(1181022009ul, generatorA.Generate());
        }

        [TestMethod]
        public void GeneratorA_generatesThirdValue_245556042()
        {
            TestHelper.GenerateMultipleTimes(generatorA, 2);
            Assert.AreEqual(245556042ul, generatorA.Generate());
        }

        [TestMethod]
        public void GeneratorA_generatesFourthValue_1744312007()
        {
            TestHelper.GenerateMultipleTimes(generatorA, 3);
            Assert.AreEqual(1744312007ul, generatorA.Generate());
        }

        [TestMethod]
        public void GeneratorA_generatesFifthValue_1352636452()
        {
            TestHelper.GenerateMultipleTimes(generatorA, 4);
            Assert.AreEqual(1352636452ul, generatorA.Generate());
        }
    }
}
