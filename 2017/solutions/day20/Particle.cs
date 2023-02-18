namespace solutions.day20
{
    public class Particle
    {
        public int ID;
        public Point Position;
        public Point Velocity;
        public readonly Point Acceleration;
        public bool IsDestroyed;

        public Particle(Point position, Point velocity, Point acceleration, int id)
        {
            Position = position;
            Velocity = velocity;
            Acceleration = acceleration;
            ID = id;
            IsDestroyed = false;
        }
    }

    public static class ParticleExtensions
    {
        public static (Particle particle, long distance) GetManhatanDistance(this Particle particle)
        {
            return (particle, particle.Position.GetManhatanDistance());
        }
    }
}
