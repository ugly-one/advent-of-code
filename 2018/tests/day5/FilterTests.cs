using NUnit.Framework;
using Solutions.day5;
using System;
namespace day5
{
    [TestFixture()]
    public class FilterTests
    {
        [Test()]
        public void TestCase()
        {
            var input = "dabAcCaCBAcCcaDA";
            var toFilterOut = 'a';

            var afterFiltering = Filter.Do(toFilterOut, input);

            Assert.AreEqual("dbcCCBcCcD", afterFiltering);
        }
    }
}
