using Common;
using NUnit.Framework;
using Solutions.day5;
using System;
using System.Linq;

namespace day5
{
    [TestFixture()]
    public class PolymerTests
    {
        [Test()]
        public void ReactOnceOnPolymerOf16Units_14UnitsLeft()
        {
            var polymer = new Polymer("dabAcCaCBAcCcaDA");

            polymer.React();

            Assert.AreEqual(14, polymer.GetActiveCount());
        }

        [Test()]
        public void React3TimesOnPolymerOf16Units_10UnitsLeft()
        {
            var polymer = new Polymer("dabAcCaCBAcCcaDA");

            var test = false;
            test = polymer.React();
            test = polymer.React();
            test = polymer.React();

            Assert.AreEqual(10, polymer.GetActiveCount());
        }

        [Test()]
        public void DoesntReact()
        {
            var polymer = new Polymer("asdfghjk");

            Assert.IsFalse(polymer.React());
        }

        [Test()]
        public void Input()
        {
            var input = FileReader.Read("../../day5/input.txt").FirstOrDefault();
            var polymer = new Polymer(input);

            var reacted = true;
            while (reacted)
            {
                reacted = polymer.React();
            }
            
            Assert.AreEqual(11540, polymer.GetActiveCount());
        }
    }
}
