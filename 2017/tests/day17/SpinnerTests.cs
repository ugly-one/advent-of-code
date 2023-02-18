using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day17;
using System.Linq;

namespace tests.day17
{
    [TestClass]
    public class SpinnerTests
    {
        private Spinner spinner;

        [TestInitialize]
        public void init()
        {
            spinner = new Spinner();
        }
        // Spinner with 3 steps per spin
        [TestMethod]
        public void OneInsert_Returns01()
        {
            var result = spinner.Spin(1, 3);

            TestHelper.AssertArrayEquals(new int[] { 0, 1 }, result);
        }

        [TestMethod]
        public void TwoInserts_Returns021()
        {
            var result = spinner.Spin(2, 3);

            TestHelper.AssertArrayEquals(new int[] { 0, 2, 1 }, result);
        }

        [TestMethod]
        public void ThreeInserts_Returns0231()
        {
            var result = spinner.Spin(3, 3);

            TestHelper.AssertArrayEquals(new int[] { 0, 2, 3, 1 }, result);
        }

        [TestMethod]
        public void NineInserts_Returns0957243861()
        {
            var result = spinner.Spin(9, 3);

            TestHelper.AssertArrayEquals(new int[] { 0, 9, 5, 7, 2, 4, 3, 8, 6, 1 }, result);
        }

        [TestMethod]
        public void _2017Inserts_ReturnsArrayWhere_638_IsAfter2017()
        {
            var result = spinner.Spin(2017, 3).ToList();

            int currentIndex = spinner.CurrentPosition;
            Assert.AreEqual(638, result[currentIndex + 1]);
        }

        //335 steps per spin
        [TestMethod]
        public void _335_StepsPerSpin_2717Inserts_ReturnsArrayWhere_1282_IsAfter2017()
        {
            spinner = new Spinner();

            var result = spinner.Spin(2017, 335).ToList() ;

            int currentIndex = spinner.CurrentPosition;
            Assert.AreEqual(1282, result[currentIndex + 1]);
        }

        [TestMethod]
        public void _335_StepsPerSpin_50000000Inserts_ReturnsArrayWhere_1282_IsAfter0()
        {
            spinner = new Spinner();

            var result = spinner.SpinAndGetAfterZero(50000000, 335);

            Assert.AreEqual(27650600, result);
        }
    }
}
