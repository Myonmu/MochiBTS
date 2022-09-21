using System;
using DG.Tweening;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Events;
using MochiBTS.Core.Primitives.Nodes;
namespace MochiBTS.Extension.Primitives
{
    public abstract class TweenerNode : ActionNode, IInterruptable, ITweener
    {

        public float duration;
        public void OnInterrupt()
        {
            Tweener.Kill();
            state = State.Success;
        }
        public Tweener Tweener { get; set; }
        public Action<Tweener> DecoratorCallback { get; set; }
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            state = State.Running;
            InitializeTweener(agent, blackboard);
            Tweener.OnComplete(() => state = State.Success);
            DecoratorCallback?.Invoke(Tweener);
            Tweener.Play();
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
            Tweener.Kill();
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            return state;
        }

        protected abstract void InitializeTweener(Agent agent, Blackboard blackboard);
    }
}