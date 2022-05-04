using System;
using DG.Tweening;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using MochiBTS.Core.Primitives.Utilities.Event;
using MochiBTS.Extension.Primitives;
using UnityEngine;
namespace MochiBTS.Extension.NodeLibrary.ActionNodes.Tween
{
    public class TweenMoveNode: ActionNode, IInterruptable, ITweener
    {
        public override string tooltip =>
            "Tweens the agent to the target location. Returns Running if tweening is not finished" +
            " and Success if finished." +
            "Can receive interrupt and once interrupted, kills the tweener and returns success.";
        public DataSource<Vector3> targetPosition;
        public float duration;
        public bool useLocal;
        public Tweener tweener { get; set; }
        public Action<Tweener> decoratorCallback { get; set; }
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            targetPosition.ObtainValue(agent,blackboard);
            
            tweener = useLocal?
                agent.transform.DOLocalMove(targetPosition.value, duration):
                agent.transform.DOMove(targetPosition.value, duration);
            tweener.OnComplete(() => state = State.Success);
            decoratorCallback.Invoke(tweener);
            tweener.Play();
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {

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
    }
}