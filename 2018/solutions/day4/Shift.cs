using System;
using System.Collections.Generic;
using System.Linq;

namespace Solutions.day4
{
    public class Shift
    {
        public readonly int Id;
        public int TotalSleepMinutes { get; private set; }
        internal readonly DateTime DateTime;
        private IList<IRecord> records;
        public IList<(int sleepBegin, int sleepEnd)> SleepFrames { get; private set; }

        public Shift(BeginShiftRecord record)
        {
            DateTime = record.DateTime;
            records = new List<IRecord>() { 
                record
            };
            Id = record.Id;
            TotalSleepMinutes = 0;
            SleepFrames = new List<(int, int)>();
        }

        internal void AddRecord(IRecord r)
        {
            if (r is WakesUpRecord)
            {
                var sleepTime = ((WakesUpRecord)r).DateTime.TimeOfDay - records.Last().DateTime.TimeOfDay;
                var minutes = sleepTime.Minutes;

                SleepFrames.Add((records.Last().DateTime.TimeOfDay.Minutes, ((WakesUpRecord)r).DateTime.TimeOfDay.Minutes));
                TotalSleepMinutes += minutes;
            }
            records.Add(r);
        }
    }
}
