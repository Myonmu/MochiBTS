using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using UnityEngine;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.General
{
    public class VariableDebugNode : ActionNode
    {

        public override string tooltip =>
            "Prints the value of a variable in the debug console. Immediately succeeds.";
        public DataSource<object> variable;
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            variable.GetValue(agent,blackboard);
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