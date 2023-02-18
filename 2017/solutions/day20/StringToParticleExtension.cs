using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using solutions.day19;

namespace solutions.day20
{
    public static class StringExtensions
    {
        public static Option<Particle> ToParticle(this string argString, int argId)
        {
            string[] chunks = argString.Split(new string[]{", "}, StringSplitOptions.None);

            Point[] points = new Point[chunks.Length];

            for (int i = 0; i < chunks.Length; i++)
            {
                var point = chunks[i].ToPoint();
                if (!point.Any()) return Option<Particle>.CreateEmpty();
                points[i] = point.First();
            }

            return Option<Particle>.Create(new Particle(
                points[0],
                points[1],
                points[2],
                argId
                ));
        }

        public static Option<Point> ToPoint(this string argString)
        {
            string[] chunks = argString.Split('<');
            if (chunks.Length != 2) return Option<Point>.CreateEmpty();

            string SubStringWithNumbers = chunks[1]; // " 2,0,0>
            SubStringWithNumbers = SubStringWithNumbers.TrimEnd('>'); // " 2,0,0"
            string[] numbersString = SubStringWithNumbers.Split(',');
            if (numbersString.Length != 3) return Option<Point>.CreateEmpty();

            long[] numbers = new long[3];
            for (int i = 0; i < 3; i++)
            {
                if (!long.TryParse(numbersString[i], out numbers[i]))
                    return Option<Point>.CreateEmpty();
            }

            return Option<Point>.Create(new Point(numbers[0], numbers[1], numbers[2]));
        }
    }
}
