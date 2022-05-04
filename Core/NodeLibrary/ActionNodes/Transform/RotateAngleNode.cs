using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using UnityEngine;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.Transform
{
    public class RotateAngleNode: ActionNode
    {
        public bool useLocalSpace;
        public DataSource<Vector3> angle;
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            angle.ObtainValue(agent,blackboard);
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {

        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            agent.transform.Rotate(angle.value, useLocalSpace? Space.Self:Space.World);
            return State.Success;
            
        }
    }
}