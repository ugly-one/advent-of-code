using System;
using System.Text.RegularExpressions;

namespace Solutions.day6
{
    public static class StringToPointParser
    {
        public static Point ToPoint(this string input)
        {
            var coordinatesRegex = new Regex(@"(?<X>\d+), (?<Y>\d+)");

            var matches = coordinatesRegex.Matches(input);

            var x = int.Parse(matches[0].Groups["X"].Value);
            var y = int.Parse(matches[0].Groups["Y"].Value);

            return new Point((uint)x, (uint)y);
        }
    }
}
