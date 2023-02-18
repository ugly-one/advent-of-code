using Common;
using NUnit.Framework;
using Solutions.day4;
using System;
using System.Collections.Generic;
using System.Linq;

namespace day4
{
    [TestFixture()]
    public class ParserTests
    {
        [Test()]
        public void TransformToRecord_ReturnsCorrectDate()
        {
            var record = Parser.TransformToRecord("[1518-11-02 00:01] Guard #1699 begins shift");

            Assert.AreEqual(new DateTime(1518, 11, 02, 0, 1, 0), record.DateTime);
        }

        [Test()]
        public void TransformToRecord_ReturnsCorrectType()
        {
            var record = Parser.TransformToRecord("[1518-04-30 00:56] falls asleep");

            Assert.AreEqual(typeof(FallsAsleepRecord), record.GetType());
        }

        [Test()]
        public void TransformToRecord_ReturnsCorrectId()
        {
            var record = Parser.TransformToRecord("[1518-04-30 00:56] Guard #1699 begins shift");

            Assert.AreEqual(1699, ((BeginShiftRecord) record).Id);
        }
    }
}
