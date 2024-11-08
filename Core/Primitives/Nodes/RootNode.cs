﻿using MochiBTS.Core.Primitives.DataContainers;
using UnityEngine;
namespace MochiBTS.Core.Primitives.Nodes
{
    public class RootNode : Node
    {
        [HideInInspector] public Node child;
        public override string Tooltip =>
            "The evaluation of a behavior tree starts from here.";

        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            return child.UpdateNode(agent, blackboard);
        }

        public override Node Clone()
        {
            var node = Instantiate(this);
            if (child is null) return node;
            node.child = child.Clone();
            node.child.AssignParent(node);
            return node;
        }
    }
}