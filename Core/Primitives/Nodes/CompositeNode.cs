using System.Collections.Generic;
using UnityEngine;
namespace MyonBTS.Core.Primitives.Nodes
{
    public abstract class CompositeNode : Node
    {
        [HideInInspector]public List<Node> children = new();

        public override Node Clone()
        {
            var node = Instantiate(this);
            node.children = children.ConvertAll(c => c.Clone());
            return node;
        }
    }
}