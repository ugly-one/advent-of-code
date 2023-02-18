using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day20;
using System.Linq;

namespace tests.day20
{
    [TestClass]
    public class ParticleUpdaterTests
    {
        [TestMethod]
        public void UpdateOne()
        {
            var particle = new Particle(
                new Point(3, 0, 0),
                new Point(2, 0, 0),
                new Point(-1, 0, 0),
                0
                );

            ParticleUpdater.Update(particle);

            Assert.AreEqual(new Point(4, 0, 0), particle.Position);
            Assert.AreEqual(new Point(1, 0, 0), particle.Velocity);
            Assert.AreEqual(new Point(-1, 0, 0), particle.Acceleration);
        }
    }
}
