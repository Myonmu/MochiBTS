using UnityEngine;
namespace MochiBTS.Core.Primitives.Nodes
{
    public abstract class ActionNode : Node
    {
        [HideInInspector]public Node parent;
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