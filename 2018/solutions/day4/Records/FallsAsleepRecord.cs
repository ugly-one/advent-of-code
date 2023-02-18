using System;

namespace Solutions.day4
{
    public class FallsAsleepRecord : IRecord
    {
        public DateTime DateTime { get; }

        public FallsAsleepRecord(DateTime dateTime)
        {
            this.DateTime = dateTime;
        }
    }
}