using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day7;

namespace tests.day7
{
    [TestClass]
    public class TowerTests
    {
        private Tower tower;
        [TestInitialize]
        public void init()
        {
            tower = new Tower();
        }
        [TestMethod]
        public void TestMethod1()
        {
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

            Node root = TowerManager.GetRootNode(tower.nodes);
            Assert.AreEqual(root.Name, "tknk");
        }

        [TestMethod]
        public void SetsWeightToANodeThatWasCreatedBeforeWeGotItsWeight()
        {
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

            Node node = tower.nodes.FirstOrDefault(n => n.Name == "ugml");
            Assert.AreEqual(node.TotalWeight, 251);
        }

        [TestMethod]
        public void addingNewNode_WithoutChildren()
        {
            tower.Add("pbga (66)");

            Assert.AreEqual("pbga", tower.nodes.First().Name);
            Assert.AreEqual(66, tower.nodes.First().TotalWeight);
        }

        [TestMethod]
        public void addingNewNode_WithOneChildren_Gives2NewNodes()
        {
            tower.Add("fwft (72) -> ktlj");

            Assert.AreEqual(2, tower.nodes.Count());
        }

        [TestMethod]
        public void addingNewNode_WithOneChildren_setsIsChildrenFine()
        {
            tower.Add("fwft (72) -> ktlj");

            Assert.AreEqual(tower.nodes.Find(n => n.Name == "fwft").IsChildren, false);
            Assert.AreEqual(tower.nodes.Find(n => n.Name == "ktlj").IsChildren, true);
        }

        [TestMethod]
        public void addingNewNode_With3Children_Gives4NewNodes()
        {
            tower.Add("padx (45) -> pbga, havc, qoyq");

            Assert.AreEqual(4, tower.nodes.Count());
        }

        [TestMethod]
        public void addingNewNode_WithExistingChildNode_UpdatesIsChildren()
        {
            tower.Add("pbga (56)");
            Assert.AreEqual(tower.nodes.Find(n => n.Name == "pbga").IsChildren, false);
            tower.Add("padx (45) -> pbga, havc, qoyq");
            Assert.AreEqual(tower.nodes.Find(n => n.Name == "pbga").IsChildren, true);
        }

        [TestMethod]
        public void FindRoot()
        {
            using (var reader = new StreamReader("../../../day7/input"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    tower.Add(line);
                }
            }

            Node root = TowerManager.GetRootNode(tower.nodes);
            Assert.AreEqual("mkxke", root.Name);
        }
    }
}
