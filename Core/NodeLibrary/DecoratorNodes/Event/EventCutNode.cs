using System.Linq;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Events;
using MochiBTS.Core.Primitives.Nodes;
namespace MochiBTS.Core.NodeLibrary.DecoratorNodes.Event
{
    public class EventCutNode : DecoratorNode, IListener
    {
        public string soEventName;
        public State outputState = State.Success;
        private bool cutoff;
        private ISubscribable soEvent;
        public override string tooltip =>
            "Executes its child normally, until the assigned Event is triggered." +
            " After the event is triggered, this node will no longer executes its child" +
            ", instead returns outputState directly. ";
        public void OnEventReceive()
        {
            cutoff = true;
        }
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            if (soEvent is null) {
                foreach (var evt in blackboard.btsEventEntries.Where(evt => evt.eventName == soEventName)) {
                    if (evt.soEvent is ISubscribable subscribable)
                        soEvent = subscribable;
                    break;
                }
            }
            soEvent?.Subscribe(this);
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
            soEvent?.Unsubscribe(this);
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            return cutoff ? outputState : child.UpdateNode(agent, blackboard);
        }
    }
}