using MyonBTS.Core.Primitives.DataContainers;
using MyonBTS.Core.Primitives.Nodes;
using MyonBTS.Core.Primitives.Utilities;
using MyonBTS.Core.Primitives.Utilities.Event;
namespace MyonBTS.Core.NodeLibrary.ActionNodes.Event
{
    public class InvokeBtsEventNode: ActionNode
    {

        public BtsEvent btsEvent;
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {

        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
            
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            if (btsEvent is null) return State.Failure;
            btsEvent.Invoke();
            return State.Success;
        }
    }
}