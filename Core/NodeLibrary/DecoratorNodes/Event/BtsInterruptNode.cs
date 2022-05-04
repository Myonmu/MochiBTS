using System;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using MochiBTS.Core.Primitives.Utilities.Event;
namespace MochiBTS.Core.NodeLibrary.DecoratorNodes.Event
{
    public class BtsInterruptNode: DecoratorNode, IListener
    {

        public BtsEvent btsEvent;
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            if (child is not IInterruptable) throw new Exception("Child node can not receive interruption");
            btsEvent += this;
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
            btsEvent -= this;
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            return child.UpdateNode(agent,blackboard);
        }


        public void OnEventReceive()
        {
            if (child is IInterruptable interruptable) {
                interruptable.OnInterrupt();
            }
        }
    }
}