using System.Collections.Generic;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using MochiBTS.Core.Primitives.Utilities;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.General
{
    public class ProcessVariableNode: ActionNode
    {

        public DataSource<object> variable;
        public List<BaseDataProcessor> processors;
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            variable.ObtainValue(agent,blackboard);
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
            
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            var newValue = variable.value;
            foreach (var processor in processors) {
                processor.Process(newValue);
            }
            variable.value = newValue;
            return State.Success;
        }
    }
}