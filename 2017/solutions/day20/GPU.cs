using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace solutions.day20
{
    public class GPU
    {
        private List<Particle> mParticles;

        public GPU(IEnumerable<Particle> particles)
        {
            mParticles = particles.ToList();
        }

        public void Run(int roundsAmount)
        {
            for (int i = 0; i < roundsAmount; i++)
            {
                mParticles.Update();
            }
        }

        public int RunWithCollide(int roundsAmount)
        {
            List<Particle> availableParticles = mParticles;
            //var sw = new Stopwatch();
            for (int i = 0; i < roundsAmount; i++)
            {
              //  sw.Restart();
                MarkParticlesAsDestroyed(availableParticles);
                //Debug.Write(sw.ElapsedMilliseconds + "  ");

                availableParticles =  availableParticles.Update();
                //Debug.WriteLine(sw.ElapsedMilliseconds + "  ");
            }

            return availableParticles.Count();
        }

        private static void MarkParticlesAsDestroyed(List<Particle> availableParticles)
        {
            var availableCount = availableParticles.Count();
            Particle particle1;
            Particle particle2;
            for (int j = 0; j < availableCount - 1; j++)
            {
                particle1 = availableParticles[j];
                if (particle1.IsDestroyed) continue;
                for (int l = j + 1; l < availableCount; l++)
                {
                    particle2 = availableParticles[l];
                    if (particle2.IsDestroyed) continue;
                    if (particle1.Position.Equals(particle2.Position))
                    {
                        particle1.IsDestroyed = true;
                        particle2.IsDestroyed = true;
                    }
                }
            }
        }

        public Particle FindClosest()
        {
            var closest = mParticles.First().GetManhatanDistance();

            foreach (var particle in mParticles.Skip(1))
            {
                var current = particle.GetManhatanDistance();
                if (current.distance < closest.distance)
                    closest = current;
            }

            return closest.particle;
        }
    }
}
