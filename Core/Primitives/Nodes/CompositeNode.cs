using System.Collections.Generic;
using UnityEngine;
namespace MochiBTS.Core.Primitives.Nodes
{
    public abstract class CompositeNode : Node
    {
        [HideInInspector] public List<Node> children = new();

        [HideInInspector]public Node parent;
        public override Node Clone()
        {
            var node = Instantiate(this);
            node.children = children.ConvertAll(c => c.Clone());
            foreach (var child in node.children) {
                child.AssignParent(node);
            }
            return node;
        }

        public override void AssignParent(Node parentNode)
        {
            parent = parentNode;
        }

        public override Node GetParent()
        {
            return parent;
        }

    }
}