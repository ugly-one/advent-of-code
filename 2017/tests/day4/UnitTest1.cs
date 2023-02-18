using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day4;

namespace tests.day4
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Validate_returnsTrueWhenNoDuplicates()
        {
            string input = "vxjtwn vjnxtw sxibvv mmws wjvtxn icawnd rprh";

            var validator = new PassphraseValidator();

            bool result = validator.Validate(input);

            Assert.IsFalse(result); // vxjtwn, wjvtxn and vjnxtw are anagrams
        }
        [TestMethod]
        public void Validate_returnsFalseWhenDuplicate()
        {
            string input = "szatel yfkve yfkve lzqhs";

            var validator = new PassphraseValidator();

            bool result = validator.Validate(input);

            Assert.IsFalse(result);
        
        }

        [TestMethod]
        public void isAnagram1(){
            var result = IEnumerableExt.isAnagram("abcde", "abcd");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void isAnagram2(){
            var result = IEnumerableExt.isAnagram("abcde", "ecdab");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void containsAnagram(){
            var words = "ioii iioi iiio";
            IEnumerable<string> w = words.Split(' ');

            var result = w.ContainsAnagram("oiii");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Validate_Returns223TimesTrue_ForChallangeInputPart2(){
            var lines = File.ReadAllLines("../../../day4/input.txt");
            var counter = 0;
            var validator = new PassphraseValidator();

            foreach (var line in lines)
            {
                if (validator.Validate(line)) counter++;
            }

            Assert.AreEqual(223, counter);
        }
    }
}
