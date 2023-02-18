using System;
using System.Text.RegularExpressions;

namespace Solutions.day3
{
    public static class Parser
    {
        public static Claim ParseClaim(string input)
        {
            var edgesRegex = new Regex(@"#(?<id>\d+) @\s(?<leftEdge>\d+),(?<topEdge>\d+)[:]\s(?<width>\d+)[x](?<height>\d+)");

            var edgesMatch = edgesRegex.Matches(input)[0];

            var id = edgesMatch.Groups["id"].Value;
            var leftEdge = edgesMatch.Groups["leftEdge"].Value;
            var topEdge = edgesMatch.Groups["topEdge"].Value;
            var width = edgesMatch.Groups["width"].Value;
            var height = edgesMatch.Groups["height"].Value;

            return new Claim(
                int.Parse(id),
                int.Parse(leftEdge), 
                int.Parse(topEdge), 
                int.Parse(width), 
                int.Parse(height));
        }
    }
}
