using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using UnityEngine;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.Transform
{
    public class MoveInDirectionNode: ActionNode
    {

        public DataSource<Vector3> direction;
        public float speed = 1;
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            direction.GetValue(agent,blackboard);
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
            
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            var agentTransform = agent.transform;
            var agentPosition = agentTransform.position;
            agentPosition += direction.value.normalized * Time.deltaTime * speed;
            agentTransform.position = agentPosition;
            return State.Running;
        }
    }
}