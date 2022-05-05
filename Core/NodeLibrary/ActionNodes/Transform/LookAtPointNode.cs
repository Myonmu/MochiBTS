using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.Transform
{
    public class LookAtPointNode: ActionNode
    {
        public bool useLocalBase;
        public DataSource<Vector3> baseDirection;
        public DataSource<Vector3> targetPoint;
        private Vector3 actualBase;
         protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            baseDirection.GetValue(agent,blackboard);
            targetPoint.GetValue(agent,blackboard);
            actualBase = useLocalBase ? agent.transform.TransformDirection(baseDirection.value) : baseDirection.value;
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            var t = agent.transform;
            t.rotation = Quaternion.FromToRotation(actualBase,targetPoint.value-t.position)*t.rotation;
            return State.Success;
        }
    }
}