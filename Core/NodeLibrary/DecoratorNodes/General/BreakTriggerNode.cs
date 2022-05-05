using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using UnityEngine;
using UnityEngine.Serialization;
namespace MochiBTS.Core.NodeLibrary.DecoratorNodes.General
{
    public class BreakTriggerNode: DecoratorNode
    {
        public State triggerState;
        public bool passThrough = true;
        public bool notEqualTo = false;
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            var childState = child.UpdateNode(agent, blackboard);
            if (!notEqualTo&& childState != triggerState||
                notEqualTo&&childState==triggerState) return passThrough?childState:State.Running;
            Debug.Break();
            return childState;
        }
    }
}