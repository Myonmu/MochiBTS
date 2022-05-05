using System.Linq;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Events;
using MochiBTS.Core.Primitives.Nodes;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.Event
{
    public class InvokeEventNode : ActionNode
    {
        public string soEventName;
        private IInvokable soEvent;
        public override string tooltip =>
            "Invokes a Scriptable Object event. The SO must implement IInvokable. Succeeds if invokable.";

        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            foreach (var evt in blackboard.btsEventEntries.Where(evt => evt.eventName == soEventName)) {
                if (evt.soEvent is IInvokable invokable)
                    soEvent = invokable;
                break;
            }
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {

        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            soEvent?.Invoke();
            return State.Success;
        }
    }
}