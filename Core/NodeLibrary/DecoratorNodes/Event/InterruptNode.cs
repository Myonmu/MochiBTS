using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using MochiBTS.Core.Primitives.Utilities.Event;
using UnityEngine;
namespace MochiBTS.Core.NodeLibrary.DecoratorNodes.Event
{
    public class InterruptNode: DecoratorNode, IListener
    {
        public override string tooltip =>
            "Calls Interrupt on the closest interruptable action node upon SO event triggering. " +
            "The SO must implement ISubscribable.";
        public ScriptableObject soEvent;
        private Node targetNode;
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            if (soEvent is ISubscribable subscribable) {
                subscribable.Subscribe(this);
            }
            if (targetNode is not null) return;
            targetNode = child;
            while (targetNode is DecoratorNode decoratorNode ) {
                targetNode = decoratorNode.child;
            }
            if (targetNode is not IInterruptable){
                Debug.LogError($"{GetType().Name}: Can't find Interruptable node to decorate.");
            }
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
            if (soEvent is ISubscribable subscribable) {
                subscribable.Unsubscribe(this);
            }
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            return child.UpdateNode(agent,blackboard);
        }
        public void OnEventReceive()
        {
            if (targetNode is IInterruptable interruptable) {
                interruptable.OnInterrupt();
            }
        }
    }
}