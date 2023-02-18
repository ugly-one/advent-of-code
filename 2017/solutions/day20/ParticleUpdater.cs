using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace solutions.day20
{
    public static class ParticleUpdater
    {
        public static void Update(this Particle argParticle)
        {
            argParticle.Velocity = argParticle.Velocity.Add(argParticle.Acceleration);
            argParticle.Position = argParticle.Position.Add(argParticle.Velocity);
        }

        public static List<Particle> Update(this IEnumerable<Particle> argParticles)
        {
            var result = new List<Particle>();
            var myLock = new object();

            Parallel.ForEach(argParticles, (item) =>
            {
                if (!item.IsDestroyed)
                {
                    item.Update();
                    lock (myLock)
                    {
                        result.Add(item);
                    }
                }
            });

            return result;
            //foreach (var item in argParticles)
            //{
            //    item.Update();
            //}
        }

    }
}