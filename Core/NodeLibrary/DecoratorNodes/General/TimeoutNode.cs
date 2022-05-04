using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using UnityEngine;
namespace MochiBTS.Core.NodeLibrary.DecoratorNodes.General
{
    public class TimeoutNode: DecoratorNode
    {
        public float duration = 1;
        private float startTime;
        public override string tooltip =>
            "Will keep running and updating its child if duration is not reached, " +
            "counting from the start of the node. Succeeds afterwards and will stop" +
            "updating its child.";
        
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            startTime = Time.time;
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            if (Time.time - startTime > duration) return State.Success;
            child.UpdateNode(agent, blackboard);
            return State.Running;
        }
    }
}