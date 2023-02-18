using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day16;

namespace tests.day16
{
    [TestClass]
    public class MovesTests
    {
        // Spin

        [TestMethod]
        public void Spin_s1_MovesLastItemToTheBeginning()
        {
            var move = new Spin(1);

            var newPosition = move.Dance("abcde");

            Assert.AreEqual("eabcd", newPosition);
        }

        [TestMethod]
        public void Spin_s3_MovesLast3ItemToTheBeginning()
        {
            var move = new Spin(3);

            var newPosition = move.Dance("abcde");

            Assert.AreEqual("cdeab", newPosition);
        }

        // Exchange

        [TestMethod]
        public void Exchange_3and4_Swaps3rdAnd4thLetter()
        {
            var move = new Exchange(3,4);

            var newPosition = move.Dance("abcde");

            Assert.AreEqual("abced", newPosition);
        }

        // Partner

        [TestMethod]
        public void Partner_EandB_SwapsEAndB()
        {
            var move = new Partner('e', 'b');

            var newPosition = move.Dance("abcde");

            Assert.AreEqual("aecdb", newPosition);
        }
    }
}
