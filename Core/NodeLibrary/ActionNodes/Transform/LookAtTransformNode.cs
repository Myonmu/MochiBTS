using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using UnityEngine;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.Transform
{
    public class LookAtTransformNode : ActionNode
    {
        public bool useLocalBase;
        public DataSource<Vector3> baseDirection;
        public DataSource<UnityEngine.Transform> targetTransform;
        private Vector3 actualBase;
        public override string Tooltip =>
            "Makes the agent's transform turns to look at the given transform. The orientation of the agent " +
            "is determined by baseDirection. No lerp. Immediately succeeds.";
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            baseDirection.GetValue(agent, blackboard);
            targetTransform.GetValue(agent, blackboard);
            actualBase = useLocalBase ? agent.transform.TransformDirection(baseDirection.value) : baseDirection.value;
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            var t = agent.transform;
            t.rotation = Quaternion.FromToRotation(actualBase, targetTransform.value.position - t.position) * t.rotation;
            return State.Success;
        }
    }
}