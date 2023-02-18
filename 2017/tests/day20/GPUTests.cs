using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day20;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace tests.day20
{
    [TestClass]
    public class GPUTests
    {
        [TestMethod]
        public void test()
        {
            var particles = readFromFile("../../../day20/TextFile1.txt");
            var gpu = new GPU(particles);

            gpu.Run(3);
            var closest = gpu.FindClosest();

            Assert.AreEqual(0, closest.ID);
        }

        [TestMethod]
        public void test2()
        {
            var particles = readFromFile("../../../day20/TextFile2.txt");
            var gpu = new GPU(particles);

            gpu.Run(10000);
            Particle closest = gpu.FindClosest();

            Assert.AreEqual(376, closest.ID);
        }

        [TestMethod]
        public void testWithCollide()
        {
            var particles = readFromFile("../../../day20/TextFile3.txt");
            var gpu = new GPU(particles);

            var survivedParticles = gpu.RunWithCollide(4);

            Assert.AreEqual(1, survivedParticles);
        }

        [TestMethod]
        public void testWithCollide2()
        {
            var particles = readFromFile("../../../day20/TextFile2.txt");
            var gpu = new GPU(particles);

            var survivedParticles = gpu.RunWithCollide(1000);

            Assert.AreEqual(574, survivedParticles);
        }

        private IEnumerable<Particle> readFromFile(string fileName)
        {
            var particles = new List<Particle>();

            using (var sr = new StreamReader(fileName))
            {
                string line;
                int counter = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    particles.Add(line.ToParticle(counter).First());
                    counter++;
                }
            }
            return particles;
        }
    }
}
