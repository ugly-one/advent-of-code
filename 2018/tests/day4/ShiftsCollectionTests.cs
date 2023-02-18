using Common;
using NUnit.Framework;
using Solutions.day4;
using System;
using System.Collections.Generic;
using System.Linq;

namespace day4
{
    [TestFixture()]
    public class ShiftsCollectionTests
    {
        [Test()]
        public void ShiftCollectionReturnsOneShiftWhen5RecordsFromOneShiftAdded()
        {
            var input = new string[]
            {
                "[1518-11-01 00:00] Guard #10 begins shift",
                "[1518-11-01 00:05] falls asleep",
                "[1518-11-01 00:25] wakes up",
                "[1518-11-01 00:30] falls asleep",
                "[1518-11-01 00:55] wakes up",
            };

            var collection = GetShiftsCollection(input);
            var shifts = collection.Shifts;
            Assert.AreEqual(1, shifts.Count());
            Assert.AreEqual(10, shifts[0].Id);
            Assert.AreEqual(45, shifts[0].TotalSleepMinutes);
        }

        [Test()]
        public void ShiftCollectionWorksFineForTestInput()
        {
            ShiftsCollection collection = GetShiftsCollectionFromFile("../../day4/testInput.txt");

            Assert.AreEqual(5, collection.Shifts.Count());

            (int lazyWorkerId, int lazyWorkerTotalTime) = collection.TheMostLazyIdAndTime();
            Assert.AreEqual(10, lazyWorkerId);
            Assert.AreEqual(50, lazyWorkerTotalTime);
            Assert.AreEqual(24, collection.GetTheMinuteAndHowManyTimes(lazyWorkerId).minute);
            Assert.AreEqual(2, collection.GetTheMinuteAndHowManyTimes(lazyWorkerId).howmany);

            Assert.AreEqual(3, collection.GetTheMinuteAndHowManyTimes(99).howmany);
            Assert.AreEqual(45, collection.GetTheMinuteAndHowManyTimes(99).minute);

        }

        [Test()]
        public void ShiftCollectionSolutionPart1Test()
        {
            ShiftsCollection collection = GetShiftsCollectionFromFile("../../day4/input.txt");

            (int lazyWorkerId, int lazyWorkerTotalTime) = collection.TheMostLazyIdAndTime();

            Assert.AreEqual(2851, lazyWorkerId);
            Assert.AreEqual(524, lazyWorkerTotalTime);
            Assert.AreEqual(44, collection.GetTheMinuteAndHowManyTimes(lazyWorkerId).minute);

        }

        [Test()]
        public void ShiftCollection_Part2TestInput()
        {
            ShiftsCollection collection = GetShiftsCollectionFromFile("../../day4/testInput.txt");

            (int id, int minute) = collection.GetMinuteMostWorkersSleep();

            Assert.AreEqual(99, id);
            Assert.AreEqual(45, minute);
            Assert.AreEqual(4455, id * minute);
        }

        [Test()]
        public void ShiftCollectionSolutionPart2Test()
        {
            ShiftsCollection collection = GetShiftsCollectionFromFile("../../day4/input.txt");

            (int id, int minute) = collection.GetMinuteMostWorkersSleep();

            Assert.AreEqual(733, id);
            Assert.AreEqual(25, minute);
            Assert.AreEqual(18325, id * minute);
        }

        private static ShiftsCollection GetShiftsCollection(IEnumerable<string> input)
        {
            IEnumerable<IRecord> records = input.Select(i => Parser.TransformToRecord(i)).OrderBy((arg) => arg.DateTime);
            var collection = new ShiftsCollection();
            foreach (var record in records)
            {
                collection.Add(record);
            }
            return collection;
        }

        private static ShiftsCollection GetShiftsCollectionFromFile(string filePath)
        {
            var input = FileReader.Read(filePath);
            return GetShiftsCollection(input);
        }
    }
}
