using System.Collections.Generic;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using MochiBTS.Core.Primitives.Utilities.DataProcessors;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.General
{
    public class ProcessVariableMultiNode: ActionNode
    {
        public DataSource<object> variableA;
        public DataSource<object> variableB;
        public DataSource<object> outputVariable;
        public List<BaseMultiDataProcessor> processors;
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            variableA.GetValue(agent,blackboard);
            variableB.GetValue(agent,blackboard);
            outputVariable.GetValue(agent,blackboard);
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
            
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            var output = outputVariable.value;
            foreach (var processor in processors) {
                output = processor.Process(variableA.value, variableB.value);
            }
            outputVariable.SetValue(output,agent,blackboard);
            return State.Success;
        }
    }
}