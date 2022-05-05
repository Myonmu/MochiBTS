using DG.Tweening;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Extension.Primitives;
using UnityEngine;
namespace MochiBTS.Extension.NodeLibrary.ActionNodes.Tween
{
    public class TweenMoveNode : TweenerNode
    {
        public DataSource<Vector3> targetPosition;
        public bool useLocal;
        public override string tooltip =>
            "Tweens the agent to the target location. Returns Running if tweening is not finished" +
            " and Success if finished." +
            "Can receive interrupt and once interrupted, kills the tweener and returns success.";
        protected override void InitializeTweener(Agent agent, Blackboard blackboard)
        {
            targetPosition.GetValue(agent, blackboard);
            tweener = useLocal ?
                agent.transform.DOLocalMove(targetPosition.value, duration) :
                agent.transform.DOMove(targetPosition.value, duration);
        }
    }
}