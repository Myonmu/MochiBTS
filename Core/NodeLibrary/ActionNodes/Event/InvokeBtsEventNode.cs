using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using MochiBTS.Core.Primitives.Utilities.Event;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.Event
{
    public class InvokeBtsEventNode:ActionNode
    {
        public override string tooltip =>
        "Invokes a built-in BtsEvent. Immediately succeeds.";
        public BtsEvent btsEvent;

        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
            
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            btsEvent.Invoke();
            return State.Success;
        }
    }
}