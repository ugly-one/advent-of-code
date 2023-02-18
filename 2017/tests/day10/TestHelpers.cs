using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace tests.day10
{
    public static class TestHelpers
    {
        public static void AssertEqual(int[] expectedList, int[] result){
            for (int i = 0; i < expectedList.Length; i++)
            {
                Assert.AreEqual(expectedList[i], result[i]);
            }
        }

        public static void AssertEqual(byte[] expectedList, byte[] result){
            for (int i = 0; i < expectedList.Length; i++)
            {
                Assert.AreEqual(expectedList[i], result[i]);
            }
        }
    }
}