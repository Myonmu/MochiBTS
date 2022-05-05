using System.Collections.Generic;
using System.Linq;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using MochiBTS.Core.Primitives.Utilities;
using UnityEngine;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.General
{
    public class ProcessVariableNode: ActionNode
    {

        public DataSource<object> variable;
        public List<BaseDataProcessor> processors;
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            variable.GetValue(agent,blackboard);
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
            
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            var newValue = processors.Aggregate
                (variable.value, (current, processor) => processor.Process(current));
            //Debug.Log(newValue);
            variable.SetValue(newValue,agent,blackboard);
            return State.Success;
        }
    }
}