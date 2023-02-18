using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day3;

namespace tests.day3
{
    [TestClass]
    public class ValueProviderTests
    {
        [TestMethod]
        public void GetValue_Returns4_whenSquareWith1_1_and2AreProvided(){
            var result = ValueProvider.GetValue(
                new Position(0, 1), 
                new Square[]{
                    new Square(new Position(0,0), 1),
                    new Square(new Position(1,0), 1),
                    new Square(new Position(1,1), 2),
                    });
            Assert.AreEqual(4, result);
        }
    }
}