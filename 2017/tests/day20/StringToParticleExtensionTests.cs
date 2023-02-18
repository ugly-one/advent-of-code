using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day20;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tests.day20
{
    [TestClass]
    public class StringToParticleExtensionTests
    {
        [TestMethod]
        public void ToParticle()
        {
            var input = "p=< 3,0,0>, v=< 2,0,0>, a=<-1,0,0>";

            Particle particle = input.ToParticle(0).First();

            Assert.AreEqual(new Point(3, 0, 0), particle.Position);
            Assert.AreEqual(new Point(2, 0, 0), particle.Velocity);
            Assert.AreEqual(new Point(-1, 0, 0), particle.Acceleration);
        }

        [TestMethod]
        public void ToPoint()
        {
            var input = " v=< 2,0,0>";

            Point point = input.ToPoint().First();

            Assert.AreEqual(new Point(2, 0, 0), point);
        }
    }
}
