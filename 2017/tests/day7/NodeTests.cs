using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using solutions.day7;

namespace tests.day7
{
    [TestClass]
    public class NodeTests
    {
        [TestMethod]
        public void Node_addNode()
        {
            var node = new Node("test");
            node.AddChildren(new Node("children"));

            Assert.AreEqual(node.Children.Count(), 1);
            Assert.AreEqual(node.Children.First().IsChildren, true);
            Assert.AreEqual(node.IsChildren, false);
        }
    }
}
