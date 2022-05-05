using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using MochiBTS.Core.Primitives.Utilities.Event;
using UnityEngine;
namespace MochiBTS.Core.NodeLibrary.DecoratorNodes.Event
{
    public class BtsCutNode: DecoratorNode, IListener
    {
        public override string tooltip =>
            "Executes its child normally, until the assigned BtsEvent is triggered." +
            " After the event is triggered, this node will no longer executes its child" +
            ", instead returns outputState directly. ";
        public BtsEvent btsEvent;
        public State outputState = State.Success;
        private bool cutoff = false;
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            btsEvent.Subscribe(this);
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
            btsEvent.Unsubscribe(this);
        }
        protected override Node.State OnUpdate(Agent agent, Blackboard blackboard)
        {
            return cutoff ?  outputState:child.UpdateNode(agent,blackboard);
        }
        public void OnEventReceive()
        {
            cutoff = true;
        }
    }
}