using MyonBTS.Core.Primitives.DataContainers;
using MyonBTS.Core.Primitives.Nodes;
using UnityEngine;
namespace MyonBTS.Core.NodeLibrary.ActionNodes.General
{
    public class DelayNode : ActionNode
    {
        public float duration = 1;
        private float startTime;
        public override string tooltip =>
            "Will keep running if duration is not reached, counting from the start of the node. Succeeds afterwards";


        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            startTime = Time.time;
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            return Time.time - startTime > duration ? State.Success : State.Running;
        }
    }
}