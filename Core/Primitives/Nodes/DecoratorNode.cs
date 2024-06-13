using Sirenix.OdinInspector;
using UnityEngine;
namespace MochiBTS.Core.Primitives.Nodes
{
    public abstract class DecoratorNode : Node
    {
        [HideInInspector] public Node child;
        [HideInInspector] public Node parent;
        public override Node Clone()
        {
            var node = Instantiate(this);
            if (child is null) return node;
            node.child = child.Clone();
            node.child.AssignParent(node);
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