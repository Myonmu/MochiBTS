using System.Linq;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Events;
using MochiBTS.Core.Primitives.Nodes;
using UnityEngine;
namespace MochiBTS.Core.NodeLibrary.DecoratorNodes.Event
{
    public class InterruptNode : DecoratorNode, IListener
    {
        public string soEventName;
        private ISubscribable soEvent;
        private Node targetNode;
        public override string Tooltip =>
            "Calls Interrupt on the closest interruptable action node upon SO event triggering. " +
            "The SO must implement ISubscribable.";
        public void OnEventReceive()
        {
            if (targetNode is IInterruptable interruptable)
                interruptable.OnInterrupt();
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
            if (targetNode is not null) return;
            targetNode = child;
            while (targetNode is DecoratorNode decoratorNode)
                targetNode = decoratorNode.child;
            if (targetNode is not IInterruptable)
                Debug.LogError($"{GetType().Name}: Can't find Interruptable node to decorate.");
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
            soEvent?.Unsubscribe(this);
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            return child.UpdateNode(agent, blackboard);
        }
    }
}