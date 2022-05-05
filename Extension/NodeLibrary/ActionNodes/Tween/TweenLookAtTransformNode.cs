using DG.Tweening;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Extension.Primitives;
using UnityEngine;
namespace MochiBTS.Extension.NodeLibrary.ActionNodes.Tween
{
    public class TweenLookAtTransformNode: TweenerNode
    {
        public DataSource<Transform> target;
        public DataSource<Vector3> baseVector;
        public AxisConstraint axisConstraint;

        protected override void InitializeTweener(Agent agent, Blackboard blackboard)
        {
            target.GetValue(agent,blackboard);
            baseVector.GetValue(agent,blackboard);
            tweener = agent.transform.DOLookAt(target.value.position, duration, axisConstraint,baseVector.value);
        }
    }
}