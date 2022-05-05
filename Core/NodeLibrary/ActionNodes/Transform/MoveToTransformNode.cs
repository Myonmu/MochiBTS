using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using UnityEngine;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.Transform
{
    public class MoveToTransformNode:ActionNode
    {
        public override string tooltip =>
            "Move the agent directly to the target transform without any navigation." +
            " Returns Running if target is not reached. Returns Success if reached. " +
            "If there is no transform to move to, returns Failure. Note That the serialized" +
            " transform field cannot be assigned, you must specify it in the Agent component." +
            "[This node uses reflection]";
        public float speed = 1;
        public float tolerance = 0.1f;
        public DataSource<UnityEngine.Transform> targetTransform;
        private float sqrTolerance;
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            sqrTolerance = tolerance * tolerance;
            targetTransform.GetValue(agent,blackboard);
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
            
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            if (targetTransform.value is null) return State.Failure;
            var agentTransform = agent.transform;
            var agentPosition = agentTransform.position;
            var direction = targetTransform.value.position - agentPosition;
            if (direction.sqrMagnitude < sqrTolerance) return State.Success;
            agentPosition += direction.normalized * Time.deltaTime * speed;
            agentTransform.position = agentPosition;
            return State.Running;
        }
    }
}