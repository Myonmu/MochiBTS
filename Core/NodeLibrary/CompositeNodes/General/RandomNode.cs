﻿using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using UnityEngine;
namespace MochiBTS.Core.NodeLibrary.CompositeNodes.General
{
    public class RandomNode : CompositeNode
    {

        public int seed;
        public override string Tooltip =>
            "Executes a random child, by using the provided seed if seed is not 0." +
            "Returns the state of the executed child.";
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            if (seed != 0) Random.InitState(seed);
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {

        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            return children[Random.Range(0, children.Count)].UpdateNode(agent, blackboard);
        }
    }
}