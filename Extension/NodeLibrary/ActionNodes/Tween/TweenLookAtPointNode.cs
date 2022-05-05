using DG.Tweening;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Extension.Primitives;
using UnityEngine;
namespace MochiBTS.Extension.NodeLibrary.ActionNodes.Tween
{
    public class TweenLookAtPointNode : TweenerNode
    {
        public DataSource<Vector3> target;
        public DataSource<Vector3> baseVector;
        public AxisConstraint axisConstraint;

        protected override void InitializeTweener(Agent agent, Blackboard blackboard)
        {
            target.GetValue(agent, blackboard);
            baseVector.GetValue(agent, blackboard);
            tweener = agent.transform.DOLookAt(target.value, duration, axisConstraint, baseVector.value);
        }
    }
}