using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using MochiBTS.Core.Primitives.Utilities.Event;
using UnityEngine;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.Event
{
    public class WaitForBtsEventNode: ActionNode, IListener
    {

        public override string tooltip =>
            "Keeps running until the assigned BtsEvent is invoked. Returns outputState afterwards.";
        public State outputState;
        public BtsEvent btsEvent;
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            state = State.Running;
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