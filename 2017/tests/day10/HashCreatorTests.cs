using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day10;

namespace tests.day10
{
    [TestClass]
    public class HashCreatorTests
    {
        [TestMethod]
        public void HashCreator_returnsCorrectListValuesForTestCase(){
            var hashCreator = new HashCreator();
            var result = hashCreator.processOneRound(new byte[]{0,1,2,3,4}, new byte[]{ 3, 4, 1, 5});
            TestHelpers.AssertEqual(new byte[]{3,4,2,1,0}, result.List);
        }

        [TestMethod]
        public void HashCreator_returnsCorrectSkipSize(){
            var hashCreator = new HashCreator();
            var result = hashCreator.processOneRound(new byte[]{0,1,2,3,4}, new byte[]{ 3, 4, 1, 5});
            Assert.AreEqual(4, result.Skip);
        }

        [TestMethod]
        public void HashCreator_returnsCorrectCurrentPosition(){
            var hashCreator = new HashCreator();
            var result = hashCreator.processOneRound(new byte[]{0,1,2,3,4}, new byte[]{ 3, 4, 1, 5});
            Assert.AreEqual(4, result.CurrentPosition);
        }

        [TestMethod]
        public void HashCreator_ForChallangeCase(){
            var hashCreator = new HashCreator();
            var list = CreateList();
            var result = hashCreator.processOneRound(list, new byte[] {206,63,255,131,65,80,238,157,254,24,133,2,16,0,1,3});

            Assert.AreEqual(9656, result.List[0]*result.List[1]);
        }

        [TestMethod]
        public void ReduceHash(){
            var hashCreator = new HashCreator();
            var result = hashCreator.ReducePartialHash(new byte[]{65 , 27 , 9 , 1 , 4 , 3 , 40 , 50 , 91 , 7 , 6 , 0 , 2 , 5 , 68 , 22});
            Assert.AreEqual(64, result);            
        }

        [TestMethod]
        public void Run_EmptyString(){
            var hashCreator = new HashCreator();
            var result = hashCreator.Run(CreateList(), "");

            Assert.AreEqual("a2582a3a0e66e6e86e3812dcb672a272", result);
        }

        [TestMethod]
        public void Run_AoC(){
            var hashCreator = new HashCreator();
            var result = hashCreator.Run(CreateList(), "AoC 2017");

            Assert.AreEqual("33efeb34ea91902bb2f59c9920caa6cd", result);
        }
        
        [TestMethod]
        public void Run_123(){
            var hashCreator = new HashCreator();
            var result = hashCreator.Run(CreateList(), "1,2,3");

            Assert.AreEqual("3efbe78a8d82f29979031a4aa0b16a9d", result);
        }
        [TestMethod]
        public void Run_124(){
            var hashCreator = new HashCreator();
            var result = hashCreator.Run(CreateList(), "1,2,4");

            Assert.AreEqual("63960835bcdc130f0b66d7ff4f6a5a8e", result);
        }
                [TestMethod]
        public void Run_Challenge(){
            var hashCreator = new HashCreator();
            var result = hashCreator.Run(CreateList(), "206,63,255,131,65,80,238,157,254,24,133,2,16,0,1,3");

            Assert.AreEqual("20b7b54c92bf73cf3e5631458a715149", result);
        }

        private byte[] CreateList(){
            var list = new byte[256];
            byte i = 0;
            while(true){
                list[i] = i;
                if (i == 255) return list;
                i++;
            }
        }
    }
}