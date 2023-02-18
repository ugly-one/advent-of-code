using System;
using System.Collections.Generic;

namespace solutions.day7
{
    public class Node{
        public string Name {get;}
        public List<Node> Children {get;}
        public bool IsChildren {get; set;}
        public int TotalWeight { get {
                int total = itsOwnWeight;
                foreach (var item in Children)
                {
                    total += item.TotalWeight;
                }
                return total;
            } }

        public int itsOwnWeight;

        public Node(string argName)
        {
            Name = argName;
            Children = new List<Node>();
            IsChildren = false;
        }

        public Node(string argName, int argWeight) : this(argName)
        {
            itsOwnWeight = argWeight;
        }

        public void SetWeight(int argWeight)
        {
            itsOwnWeight = argWeight;
        }

        public void AddChildren(Node node)
        {
            Children.Add(node);
            node.IsChildren = true;
        }
    }
}