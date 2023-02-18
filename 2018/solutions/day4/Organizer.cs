using System;
using System.Collections.Generic;
using System.Linq;

namespace Solutions.day4
{
    public static class Organizer
    {
        public static IEnumerable<IRecord> Sort(IEnumerable<IRecord> records)
        {
            return records.OrderBy((arg) => arg.DateTime);
        }
    }
}
