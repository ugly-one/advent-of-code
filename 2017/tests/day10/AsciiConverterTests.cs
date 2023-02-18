using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day10;

namespace tests.day10
{
    [TestClass]
    public class AsciiConverterTests
    {
        [TestMethod]
        public void ConvertToAscii(){
            byte[] result = "1,2,3".ConvertToAscii();
            TestHelpers.AssertEqual(new byte[]{49,44,50,44,51}, result);
        }
    }
}