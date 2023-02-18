using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day16;

namespace tests.day16
{
    [TestClass]
    public class MoveConverterTests
    {
        [TestMethod]
        public void AddMoveS1_AddsSpinMove()
        {
            var move = MoveConverter.Convert("s1") as Spin;

            Assert.AreEqual(1, move.SpinValue);
        }

        [TestMethod]
        public void AddMoveX3And4_AddsExchangeMove()
        {
            var move = MoveConverter.Convert("x3/4") as Exchange;
            Assert.AreEqual(3, move.FirstPosition);
            Assert.AreEqual(4, move.SeconPosition);
        }

        [TestMethod]
        public void AddMovePEAndB_AddsPartnerMove()
        {
            var move = MoveConverter.Convert("pe/b") as Partner;
            Assert.AreEqual('e', move.FirstChar);
            Assert.AreEqual('b', move.SecondChar);
        }
    }
}
