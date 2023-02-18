using System;
using System.Collections.Generic;
using System.Linq;

namespace solutions.day7
{
    public class Tower
    {
        public List<Node> nodes;
        public Tower()
        {
            nodes = new List<Node>();
        }

        public void Add(string line)
        {
            var chunks = line.Split('-');
            if (chunks.Length == 0) return;

            string[] nameAndWegith = chunks[0].Split(' ');
            string name = nameAndWegith[0];
            string weightString = nameAndWegith[1].Trim('(', ')');
            int.TryParse(weightString, out int weight);

            Node nodeToAdd = (nodes.Find(n => n.Name == name));
            if (nodeToAdd == null)
            {
                nodeToAdd = new Node(name, weight);
                nodes.Add(nodeToAdd);
            }
            else
            {
                nodeToAdd.SetWeight(weight);
            }
            // new node no children
            if (chunks.Length > 1)
            {
                var childrenString = chunks[1].TrimStart(new char[] { '>', ' ' });
                var children = childrenString.Split(',').Select(s => s.Trim());

                foreach (var childName in children)
                {
                    var existingNode = nodes.Find(n => n.Name == childName);
                    if (existingNode == null)
                    {
                        var newChildrenNode = new Node(childName);
                        newChildrenNode.IsChildren = true;
                        nodeToAdd.Children.Add(newChildrenNode);
                        nodes.Add(newChildrenNode);
                    }
                    else
                    {
                        nodeToAdd.Children.Add(existingNode);
                        existingNode.IsChildren = true;
                    }
                }
            }

        }

    }
}
