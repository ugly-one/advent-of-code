using System;

namespace Solutions.day4
{
    public class WakesUpRecord : IRecord
    {
        public DateTime DateTime { get; }

        public WakesUpRecord(DateTime dateTime)
        {
            this.DateTime = dateTime;
        }
    }
}