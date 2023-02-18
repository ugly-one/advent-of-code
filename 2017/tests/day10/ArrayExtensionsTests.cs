using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day10;

namespace tests.day10
{
    [TestClass]
    public class ArrayExtensionsTests
    {
        [TestMethod]
        public void ReversePart_reverses_first3Digits()
        {
            var list = new byte[]{0,1,2,3,4};
            list.ReversePart(0, 3);
            TestHelpers.AssertEqual(new byte[]{2,1,0,3,4}, list);                 
        }
        [TestMethod]
        public void ReversePart_reverses_last3Digits()
        {
            byte[] list = new byte[]{0,1,2,3,4};
            list.ReversePart(2, 3);
            TestHelpers.AssertEqual(new byte[]{0,1,4,3,2}, list);        
        }

        [TestMethod]
        public void ReversePart_reverses_last1AndFirst2Digits()
        {
            byte[] list = new byte[]{0,1,2,3,4};
            list.ReversePart(4, 3);
            TestHelpers.AssertEqual(new byte[]{0,4,2,3,1}, list);        
        }

        [TestMethod]
        public void ReversePart_reverses_last2AndFirst1Digit()
        {
            byte[] list = new byte[]{0,1,2,3,4};
            list.ReversePart(3, 3);
            TestHelpers.AssertEqual(new byte[]{3,1,2,0,4}, list);        
        }

        [TestMethod]
        public void Add_addsToTheEnd(){
            var list = new byte[]{49,44,50,44,51};
            var result = list.Add(new byte[]{17, 31, 73, 47, 23});

            TestHelpers.AssertEqual(new byte[]{49,44,50,44,51,17,31,73,47,23}, result);
        }
    }
}
