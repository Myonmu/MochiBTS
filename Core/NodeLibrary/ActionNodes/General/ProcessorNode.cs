using System.Collections.Generic;
using MyonBTS.Core.Primitives.DataContainers;
using MyonBTS.Core.Primitives.Nodes;
using MyonBTS.Core.Primitives.Utilities;
namespace MyonBTS.Core.NodeLibrary.ActionNodes.General
{
    public class ProcessorNode: ActionNode
    {

        
        public List<BaseDataProcessor> dataProcessors = new();
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
            
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            foreach (var processor in dataProcessors) {
                processor.Process(agent);
            }
            return State.Success;
        }
    }
}