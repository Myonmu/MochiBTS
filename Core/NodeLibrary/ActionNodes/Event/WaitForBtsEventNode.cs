using MyonBTS.Core.Primitives.DataContainers;
using MyonBTS.Core.Primitives.Nodes;
using MyonBTS.Core.Primitives.Utilities.Event;
namespace MyonBTS.Core.NodeLibrary.ActionNodes.Event
{
    public class WaitForBtsEventNode: ActionNode, IListener
    {

        public State outputState;
        public BtsEvent btsEvent;
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            btsEvent += this;
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
            btsEvent -= this;
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            return state;
        }
        public void OnEventReceive()
        {
            state = outputState;
        }
    }
}