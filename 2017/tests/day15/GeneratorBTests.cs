using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day15;

namespace tests.day15
{
    [TestClass]
    public class GeneratorBTests
    {
        private Generator generatorB;

        [TestInitialize]
        public void Init()
        {
            generatorB = new Generator(8921, 48271);
        }

        [TestMethod]
        public void GeneratorB_generatesFirstValue()
        {
            Assert.AreEqual(430625591ul, generatorB.Generate());
        }

        [TestMethod]
        public void GeneratorB_generatesSecondValue()
        {
            TestHelper.GenerateMultipleTimes(generatorB, 1);
            Assert.AreEqual(1233683848ul, generatorB.Generate());
        }

        [TestMethod]
        public void GeneratorB_generatesThirdValue()
        {
            TestHelper.GenerateMultipleTimes(generatorB, 2);
            Assert.AreEqual(1431495498ul, generatorB.Generate());
        }

        [TestMethod]
        public void GeneratorB_generatesFourthValue()
        {
            TestHelper.GenerateMultipleTimes(generatorB, 3);
            Assert.AreEqual(137874439ul, generatorB.Generate());
        }

        [TestMethod]
        public void GeneratorB_generatesFifthValue()
        {
            TestHelper.GenerateMultipleTimes(generatorB, 4);
            Assert.AreEqual(285222916ul, generatorB.Generate());
        }
    }
}
