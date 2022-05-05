﻿using UnityEngine;
namespace MochiBTS.Core.Primitives.Nodes
{
    public abstract class DecoratorNode : Node
    {
        [HideInInspector] public Node child;

        public override Node Clone()
        {
            var node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }
    }
}