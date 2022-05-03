using MyonBTS.Core.Primitives.DataContainers;
using MyonBTS.Core.Primitives.Nodes;
using MyonBTS.Core.Primitives.Utilities;
using MyonBTS.Core.Primitives.Utilities.Event;
using UnityEngine;
namespace MyonBTS.Core.NodeLibrary.ActionNodes.Event
{
    public class InvokeInvokableEventNode : ActionNode
    {
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