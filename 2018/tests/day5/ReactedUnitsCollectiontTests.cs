using NUnit.Framework;
using Solutions.day5;
using System.Collections.Generic;
using System.Linq;

namespace day5
{
    [TestFixture()]
    public class ReactedUnitsCollectiontTests
    {
        [Test()]
        public void Test()
        {
            var reactedUnitsCollection = new ReactedUnitsCollection(10);
            int index = reactedUnitsCollection.GetNextAvailable();

            Assert.AreEqual(0, index);
        }

        [Test()]
        public void Test2()
        {
            var reactedUnitsCollection = new ReactedUnitsCollection(10);
            int index = reactedUnitsCollection.GetNextAvailable();
            index = reactedUnitsCollection.GetNextAvailable();
            index = reactedUnitsCollection.GetNextAvailable();

            Assert.AreEqual(2, index);
        }

        [Test()]
        public void TestAfterRemovingFirstIndex()
        {
            var reactedUnitsCollection = new ReactedUnitsCollection(10);

            reactedUnitsCollection.React(0, 1);
            int index = reactedUnitsCollection.GetNextAvailable();

            Assert.AreEqual(2, index);
        }

        [Test()]
        public void TestAfterRemoving2_3index()
        {
            var reactedUnitsCollection = new ReactedUnitsCollection(10);

            reactedUnitsCollection.React(2,3);
            int index = reactedUnitsCollection.GetNextAvailable();
            Assert.AreEqual(0, index);
            index = reactedUnitsCollection.GetNextAvailable();
            Assert.AreEqual(1, index);
            index = reactedUnitsCollection.GetNextAvailable();
            Assert.AreEqual(4, index);
        }
    }
}
