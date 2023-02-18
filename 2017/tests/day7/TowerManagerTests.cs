using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day7;

namespace tests.day7
{
    [TestClass]
    public class TowerManagerTests
    {
        private Tower tower;
        [TestInitialize]
        public void init()
        {
            tower = new Tower();
            var input = new List<string>{
                "pbga (66)",
                "xhth (57)",
                "ebii (61)",
                "havc (66)",
                "ktlj (57)",
                "fwft (72) -> ktlj, cntj, xhth",
                "qoyq (66)",
                "padx (45) -> pbga, havc, qoyq",
                "tknk (41) -> ugml, padx, fwft",
                "jptl (61)",
                "ugml (68) -> gyxo, ebii, jptl",
                "gyxo (61)",
                "cntj (57)"
            };

            foreach (var line in input)
            {
                tower.Add(line);
            }
        }

        [TestMethod]
        public void IsInBallance_returnsTrue()
        {
            var result = TowerManager.IsBallanced(tower.nodes, "padx");
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void IsInBallance_returnsTrue2()
        {
            var result = TowerManager.IsBallanced(tower.nodes, "fwft");
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void IsInBallance_returnsFalse()
        {
            var result = TowerManager.IsBallanced(tower.nodes,"ugml");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FindUnbalanced()
        {
            Node unBalancedNode = TowerManager.FindUnbalanced(tower.nodes);

            Assert.AreEqual("tknk", unBalancedNode.Name);
        }

        [TestMethod]
        public void GetCorrectWeight()
        {
            Node unBalancedNode = TowerManager.FindUnbalanced(tower.nodes);
            int correctWeight = TowerManager.GetCorrectWeight( unBalancedNode);
            Assert.AreEqual(60, correctWeight);
        }

        [TestMethod]
        public void FindRoot()
        {
            tower = new Tower();

            using (var reader = new StreamReader("../../../day7/input"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    tower.Add(line);
                }
            }
            Node unbalancedNode = TowerManager.FindUnbalanced(tower.nodes);
            int correctWeight = TowerManager.GetCorrectWeight(unbalancedNode);
            Assert.AreEqual(268, correctWeight);
        }
    }
}
