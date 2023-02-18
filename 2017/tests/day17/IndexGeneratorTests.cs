using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day17;
using System.Collections.Generic;
using System.Linq;

namespace tests.day17
{
    [TestClass]
    public class IndexGeneratorTests
    {
        [TestMethod]
        public void Generate()
        {
            IEnumerable<int> indexes = IndexGenerator.Generate(3).Take(9);

            TestHelper.AssertArrayEquals(indexes, new int[] { 1, 1, 2, 2, 1 , 5, 2, 6, 1});
        }
    }
}
