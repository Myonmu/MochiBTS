using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using UnityEngine;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.Transform
{
    public class MoveToPointNode: ActionNode
    {
        public override string tooltip =>
            "Move the agent directly to the target Vector3 without any navigation." +
            " Returns Running if target is not reached. Returns Success if reached. " +
            "[This node uses reflection]";
        public float speed = 1;
        public float tolerance = 0.1f;
        private float sqrTolerance;
        public DataSource<Vector3> destination;

        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            destination.GetValue(agent,blackboard);
            sqrTolerance = tolerance * tolerance;
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
       
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            var agentTransform = agent.transform;
            var agentPosition = agentTransform.position;
            var direction = destination.value - agentPosition;
            if (direction.sqrMagnitude < sqrTolerance) return State.Success;
            agentPosition += direction.normalized * Time.deltaTime * speed;
            agentTransform.position = agentPosition;
            return State.Running;
        }
    }
}