using System;
using DG.Tweening;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using MochiBTS.Core.Primitives.Utilities.Event;
namespace MochiBTS.Extension.Primitives
{
    public abstract class TweenerNode : ActionNode,IInterruptable, ITweener
    {

        public float duration;
        public Tweener tweener { get; set; }
        public Action<Tweener> decoratorCallback { get; set; }
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            state = State.Running;
            InitializeTweener(agent,blackboard);
            tweener.OnComplete(() => state = State.Success);
            decoratorCallback?.Invoke(tweener);
            tweener.Play();
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
            tweener.Kill();
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            return state;
        }
        public void OnInterrupt()
        {
            tweener.Kill();
            state = State.Success;
        }

        protected abstract void InitializeTweener(Agent agent, Blackboard blackboard);

    }
}