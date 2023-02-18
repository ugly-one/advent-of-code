using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day16;
using System.Collections.Generic;
using System.IO;

namespace tests.day16
{
    [TestClass]
    public class MovesManagerTests
    {
        [TestMethod]
        public void test1()
        {
            MovesManager manager = Initialize(5, "../../../day16/testData.txt");

            manager.DoAllMoves();

            Assert.AreEqual("baedc", manager.CurrentPositions);
        }

        [TestMethod]
        public void test2()
        {
            MovesManager movesManager = Initialize(16, "../../../day16/ChallangeTestData.txt");
            DanceManager danceManager = new DanceManager(movesManager);

            danceManager.DanceTimes(1);

            Assert.AreEqual("bijankplfgmeodhc", danceManager.CurrentPositions);
        }

        [TestMethod]
        public void testBillionDances()
        {
            MovesManager movesManager = Initialize(16, "../../../day16/ChallangeTestData.txt");
            DanceManager danceManager = new DanceManager(movesManager);

            danceManager.DanceTimes(1000000000);

            Assert.AreEqual("bpjahknliomefdgc", danceManager.CurrentPositions);
        }

        private MovesManager Initialize(int size, string fileName)
        {
            var manager = new MovesManager(size);
            var moves = new List<Move>();

            using (var sr = new StreamReader(fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    moves.Add(MoveConverter.Convert(line));
                }
            }
            manager.SetMoves(moves);
            return manager;
        }
    }
}
