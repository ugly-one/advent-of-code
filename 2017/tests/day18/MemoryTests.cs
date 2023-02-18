using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day18;
using System.Linq;

namespace tests.day18
{
    [TestClass]
    public class MemoryTests
    {
        private Memory assembler;

        [TestInitialize]
        public void Init()
        {
            assembler = new Memory();
        }
        [TestMethod]
        public void Set()
        {
            assembler.Set('a', "4");

            Assert.AreEqual(assembler.Registers.Where(r => r.Name == 'a').First().Value, 4);
        }

        [TestMethod]
        public void Add_CreatesWhenNonExisting()
        {
            assembler.Add('a', "5");

            Assert.AreEqual(assembler.Registers.Where(r => r.Name == 'a').First().Value, 5);
        }

        [TestMethod]
        public void Add_adds()
        {
            assembler.Set('a', "4");
            assembler.Add('a', "5");

            Assert.AreEqual(assembler.Registers.Where(r => r.Name == 'a').First().Value, 9);
        }

        [TestMethod]
        public void Mul_multiplies()
        {
            assembler.Set('a', "4");
            assembler.Multiply('a', "2");

            Assert.AreEqual(assembler.Registers.Where(r => r.Name == 'a').First().Value, 8);
        }

        [TestMethod]
        public void Mod()
        {
            assembler.Set('a', "4");
            assembler.Mod('a', "3");

            Assert.AreEqual(assembler.Registers.Where(r => r.Name == 'a').First().Value, 1);
        }
    }
}
