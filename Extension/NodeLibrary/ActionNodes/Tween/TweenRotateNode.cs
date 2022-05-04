using System;
using DG.Tweening;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using MochiBTS.Core.Primitives.Utilities.Event;
using MochiBTS.Extension.Primitives;
using UnityEngine;
namespace MochiBTS.Extension.NodeLibrary.ActionNodes.Tween
{
    public class TweenRotateNode: ActionNode,IInterruptable,ITweener
    {
        public DataSource<Vector3> targetRotation;
        public float duration;
        public bool useLocal;
        
        public Tweener tweener { get; set; }
        public Action<Tweener> decoratorCallback { get; set; }
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            targetRotation.ObtainValue(agent,blackboard);
            tweener = useLocal?
                agent.transform.DOLocalRotate(targetRotation.value, duration):
                agent.transform.DORotate(targetRotation.value, duration);
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