using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace tests.day17
{
    public static class TestHelper
    {
        public static void AssertArrayEquals(IEnumerable<int> expected, IEnumerable<int> observed)
        {
            var expectedList = expected.ToList();
            var observedList = observed.ToList();
            for (int i = 0; i < expectedList.Count; i++)
            {
                Assert.AreEqual(expectedList[i], observedList[i]);
            }
        }
    }
}
