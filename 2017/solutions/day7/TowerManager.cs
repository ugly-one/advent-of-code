using System;
using System.Collections.Generic;
using System.Linq;

namespace solutions.day7
{
    public static class TowerManager
    {
        public static bool IsBallanced(IEnumerable<Node> argNodes, string argNodeName)
        {
            Node node = argNodes.FirstOrDefault(n => n.Name == argNodeName);

            int childerAmount = node.Children.Count;
            if (childerAmount <= 1) return true;

            int anotherChildrenWeight = node.Children.Take(1).FirstOrDefault().TotalWeight;

            foreach (var child in node.Children.Skip(1))
            {
                if (child.TotalWeight != anotherChildrenWeight) return false;
            }

            return true;
        }

        public static Node FindUnbalanced(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                if (!IsBallanced(nodes, node.Name)){
                    return node;
                }
            }
            return null;
        }

        public static int GetCorrectWeight( Node argUnbalancedNode)
        {
            Node[] differentChildren = new Node[2] { argUnbalancedNode.Children[0], null };

            foreach (var child in argUnbalancedNode.Children.Skip(1))
            {
                if (child.TotalWeight != differentChildren[0].TotalWeight)
                {
                    differentChildren[1] = child;
                    break;
                }
            }

            int difference = differentChildren[0].TotalWeight - differentChildren[1].TotalWeight;

            var itemsWithTheSameWeight = argUnbalancedNode.Children.Where(n => n.TotalWeight == differentChildren[0].TotalWeight);

            if (itemsWithTheSameWeight.Count() == 1) return itemsWithTheSameWeight.First().itsOwnWeight - difference;
            return differentChildren[1].itsOwnWeight + difference;
        }

        public static Node GetRootNode(IEnumerable<Node> argNodes)
        {
            var root = argNodes.FirstOrDefault(n => n.IsChildren == false);
            return root;
        }
    }
}
