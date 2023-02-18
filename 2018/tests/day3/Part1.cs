using Common;
using NUnit.Framework;
using Solutions.day3;
using System;
using System.Linq;

namespace day3
{
    [TestFixture()]
    public class Part1
    {
        [Test()]
        public void GetOverlappedCountReturns4()
        {
            var input = new string[]{
             "#1 @ 1,3: 4x4",
             "#2 @ 3,1: 4x4",
             "#3 @ 5,5: 2x2"
            };

            var fabric = new Fabric(8);

            fabric.Mark(input);

            var result = fabric.GetOverlappedCount();

            Assert.AreEqual(4, result);
        }

        [Test()]
        public void ParserClaimsTest()
        {
            var input = "#1 @ 1,3: 4x5";

            var claim = Parser.ParseClaim(input);

            Assert.AreEqual(1, claim.LeftEdge);
            Assert.AreEqual(3, claim.TopEdge);
            Assert.AreEqual(4, claim.Width);
            Assert.AreEqual(5, claim.Height);
            Assert.AreEqual(1, claim.Id);
        }

        [Test()]
        public void Part1Solution()
        {
            var input = FileReader.Read("../../day3/input.txt");

            var fabric = new Fabric(1000);

            fabric.Mark(input.ToArray());

            Assert.AreEqual(117505, fabric.GetOverlappedCount());
        }

        [Test()]
        public void GetIdOfNonOverlappingSquareReturns3()
        {
            var input = new string[]{
             "#1 @ 1,3: 4x4",
             "#2 @ 3,1: 4x4",
             "#3 @ 5,5: 2x2"
            };

            var fabric = new Fabric(8);

            fabric.Mark(input);

            Assert.AreEqual(3, fabric.GetIdOfNonOverlappingClaims());
        }

        [Test()]
        public void Part2Solution()
        {
            var input = FileReader.Read("../../day3/input.txt");

            var fabric = new Fabric(1000);

            fabric.Mark(input.ToArray());

            Assert.AreEqual(1254, fabric.GetIdOfNonOverlappingClaims());
        }
    }
}
