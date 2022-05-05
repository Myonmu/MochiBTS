using DG.Tweening;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Extension.Primitives;
using UnityEngine;
namespace MochiBTS.Extension.NodeLibrary.ActionNodes.Tween
{
    public class TweenScaleNode:  TweenerNode
    {
        public DataSource<Vector3> targetScale;
        protected override void InitializeTweener(Agent agent, Blackboard blackboard)
        {
            targetScale.GetValue(agent,blackboard);
            tweener = agent.transform.DOScale(targetScale.value, duration);
        }
    }
}