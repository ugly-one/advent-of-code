using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Solutions.day4
{
    public static class Parser
    {

        public static IRecord TransformToRecord(string input)
        {
            var regexDate = new Regex(@"(?<year>\d+)-(?<month>\d+)-(?<day>\d+) (?<hour>\d+):(?<minute>\d+)]");
            var fallsAsleepRegex = new Regex(@"falls asleep");
            var wakesUpRegex = new Regex(@"wakes up");
            var beginsShiftRegex = new Regex(@"Guard #(?<id>\d+) begins shift");

            var match = regexDate.Matches(input);
            var m = match[0]; // take the first match - assume we always have a date
            var year = int.Parse(m.Groups["year"].Value);
            var month = int.Parse(m.Groups["month"].Value);
            var day = int.Parse(m.Groups["day"].Value);
            var hour = int.Parse(m.Groups["hour"].Value);
            var minute = int.Parse(m.Groups["minute"].Value);

            var dateTime = new DateTime(year, month, day, hour, minute, 0);


            var fallsMatch = fallsAsleepRegex.Matches(input);
            if (fallsMatch.Count > 0)
            {
                return new FallsAsleepRecord(dateTime);
            }

            var wakesUpMatch = wakesUpRegex.Matches(input);
            if (wakesUpMatch.Count > 0)
            {
                return new WakesUpRecord(dateTime);
            }

            // if we are here - it means it must be a begin shift record

            var beginShiftMatch = beginsShiftRegex.Matches(input);
            int id = int.Parse(beginShiftMatch[0].Groups["id"].Value);

            return new BeginShiftRecord(dateTime, id);
        }
    }
}
