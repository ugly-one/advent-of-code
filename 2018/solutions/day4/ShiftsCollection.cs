using System;
using System.Collections.Generic;
using System.Linq;

namespace Solutions.day4
{
    public class ShiftsCollection
    {
        public readonly IList<Shift> Shifts;

        public ShiftsCollection()
        {
            Shifts = new List<Shift>();
        }

        public (int id, int totalTile) TheMostLazyIdAndTime()
        {
            IEnumerable<(int id, int totalTime)> idTotalTileTuple = Shifts.GroupBy((arg) => arg.Id).Select(g => (g.Key, g.Sum(s => s.TotalSleepMinutes)));
            return idTotalTileTuple.OrderBy(a => a.totalTime).Last();
        }

        // gets the minute the given worker (id) spends most of his time asleep
        public (int minute, int howmany) GetTheMinuteAndHowManyTimes(int id)
        {
            var workersShifts = Shifts.Where(s => s.Id == id);
            int[] minutes = new int[60];

            foreach (var shift in workersShifts)
            {
                var frames = shift.SleepFrames;
                foreach (var frame in frames)
                {
                    for (int minute = frame.sleepBegin; minute < frame.sleepEnd; minute++)
                    {
                        minutes[minute]++;
                    }
                }
            }
            var max = minutes.Max();
            return (minutes.ToList().IndexOf(max) , max);
        }

        public (int id, int minute) GetMinuteMostWorkersSleep()
        {
            // get the list of IDs
            var ids = Shifts.GroupBy(s => s.Id).Select(g => g.Key);
            // run GetTheMinuteAndHowManyTimes(id) for each worker
            Match bestMatch = null;
            foreach (var id in ids)
            {
                (int minute, int howmanytimes) = GetTheMinuteAndHowManyTimes(id);

                // check if the currect worker is "better" than the best one
                if (bestMatch is null)
                {
                    bestMatch = new Match(id, minute, howmanytimes);
                }
                else
                {
                    if (howmanytimes > bestMatch.HowOften)
                    {
                        bestMatch = new Match(id, minute, howmanytimes);
                    }
                }
            }

            return (bestMatch.Id, bestMatch.Minute);
        }

        private class Match
        {
            public Match(int id, int minute, int howOften)
            {
                Id = id;
                Minute = minute;
                HowOften = howOften;
            }

            public int Id { get; }
            public int Minute { get; }
            public int HowOften { get; }

        }

        public void Add(IRecord input)
        {
            if (input is BeginShiftRecord a)
            {
                var shift = new Shift(a);
                Shifts.Add(shift);
            }
            else 
            {
                Shifts.Last().AddRecord(input);
            }
        }
    }
}
