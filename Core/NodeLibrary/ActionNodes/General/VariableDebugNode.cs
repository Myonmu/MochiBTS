using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using UnityEngine;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.General
{
    public class VariableDebugNode : ActionNode
    {
        public DataSource<object> variable;

        public override string Tooltip =>
            "Prints the value of a variable in the debug console. Immediately succeeds.";
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            variable.GetValue(agent, blackboard);
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {

        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            Debug.Log(variable.value);
            return State.Success;
        }
    }
}