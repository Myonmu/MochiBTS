using DG.Tweening;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using MochiBTS.Core.Primitives.Utilities.Event;
using UnityEngine;
namespace MochiBTS.Extension.Tween.NodeLibrary.ActionNodes
{
    public class TweenMoveNode: ActionNode, IInterruptable
    {
        public DataSource<Vector3> targetPosition;
        public float duration;
        public AnimationCurve curve;
        private Tweener tweener;
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            targetPosition.ObtainValue(agent,blackboard);
            tweener = agent.transform.DOMove(targetPosition.value, duration).
                OnComplete(()=>state = State.Success).SetEase(curve);
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
        }
    }
}