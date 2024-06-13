using System;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
namespace MochiBTS.Core.NodeLibrary.DecoratorNodes.General
{
    public class NegateNode : DecoratorNode
    {

        public override string Tooltip =>
            "Return the opposite state (Success <-> Failure). Running state is returned as normal";
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {

        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {

        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            var childState = child.UpdateNode(agent, blackboard);
            return childState switch {
                State.Running => State.Running,
                State.Failure => State.Success,
                State.Success => State.Failure,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}