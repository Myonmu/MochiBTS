using System;
using DG.Tweening;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using MochiBTS.Core.Primitives.Utilities.Event;
using MochiBTS.Extension.Primitives;
using UnityEngine;
namespace MochiBTS.Extension.NodeLibrary.ActionNodes.Tween
{
    public class TweenRotateNode: TweenerNode
    {
        public DataSource<Vector3> targetRotation;
        public bool useLocal;
        protected override void InitializeTweener(Agent agent, Blackboard blackboard)
        {
            targetRotation.GetValue(agent,blackboard);
            tweener = useLocal?
                agent.transform.DOLocalRotate(targetRotation.value, duration):
                agent.transform.DORotate(targetRotation.value, duration);
        }
    }
}