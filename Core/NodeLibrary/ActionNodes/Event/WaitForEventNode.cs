using System.Linq;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Events;
using MochiBTS.Core.Primitives.Nodes;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.Event
{
    public class WaitForEventNode : ActionNode, IListener
    {
        public State outputState;
        public string soEventName;
        private ISubscribable soEvent;

        public override string tooltip =>
            "Keeps running until the assigned Event is invoked. Returns outputState afterwards.";
        public void OnEventReceive()
        {
            state = outputState;
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
            state = State.Running;
            soEvent?.Subscribe(this);
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
            soEvent?.Unsubscribe(this);
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            return state;
        }
    }
}