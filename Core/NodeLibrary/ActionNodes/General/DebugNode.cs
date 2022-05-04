using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using UnityEngine;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.General
{
    public class DebugNode : ActionNode
    {
        public string message;

        public override string tooltip =>
            "Prints a message in the debug console. Immediately succeeds after print.";
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {

        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {

        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            Debug.Log(message);
            return State.Success;
        }
    }
}