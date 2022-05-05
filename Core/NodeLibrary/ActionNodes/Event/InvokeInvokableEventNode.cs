using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using MochiBTS.Core.Primitives.Utilities.Event;
using UnityEngine;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.Event
{
    public class InvokeInvokableEventNode : ActionNode
    {
        public override string tooltip =>
            "Invokes a Scriptable Object event. The SO must implement IInvokable. " +
            "Results in Failure if the SO cannot be invoked. Succeeds if invokable.";
        public ScriptableObject soEvent;

        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
            
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            if (soEvent is not IInvokable invokable) return State.Failure;
            invokable.Invoke();
            return State.Success;
        }
    }
}