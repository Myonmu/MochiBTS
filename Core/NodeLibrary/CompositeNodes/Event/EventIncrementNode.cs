using System.Linq;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Events;
using MochiBTS.Core.Primitives.Nodes;
namespace MochiBTS.Core.NodeLibrary.CompositeNodes.Event
{
    public class EventIncrementNode : CompositeNode, IListener
    {
        public string soEventName;
        private int currentChildIndex;
        private ISubscribable soEvent;

        public override string Tooltip =>
            "Executes its children from left to right, returns Running. " +
            "Will only switch to the next child upon reception" +
            " of the assigned btsEvent. Returns success if reaches the end.";

        public void OnEventReceive()
        {
            currentChildIndex++;
        }
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            currentChildIndex = 0;
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
            if (currentChildIndex >= children.Count) return State.Success;
            children[currentChildIndex].UpdateNode(agent, blackboard);
            return State.Running;
        }

        public override void UpdateInfo()
        {
            info = $"{soEventName}>>{currentChildIndex.ToString()}~{children[currentChildIndex].name.Replace("Node","")}";
        }
    }
}