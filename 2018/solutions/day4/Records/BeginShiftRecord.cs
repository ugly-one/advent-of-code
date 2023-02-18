using System;

namespace Solutions.day4
{
    public class BeginShiftRecord : IRecord
    {
        public DateTime DateTime { get; }
        public int Id { get; }

        public BeginShiftRecord(DateTime dateTime, int id)
        {
            this.DateTime = dateTime;
            this.Id = id;
        }
    }
}