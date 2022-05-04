using DG.Tweening;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using UnityEngine;
namespace MochiBTS.Extension.Primitives
{
    public abstract class TweenDecoratorNode: DecoratorNode
    {
        protected Node targetNode;
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            targetNode = child;
            while (targetNode is DecoratorNode decoratorNode ) {
                targetNode = decoratorNode.child;
            }
            if (targetNode is ITweener tweenerNode) {
                tweenerNode.decoratorCallback += DecorateTweener;
            } else {
                Debug.LogError($"{GetType().Name}: Can't find tweener to decorate.");
            }
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
            if (targetNode is ITweener tweenerNode) {
                tweenerNode.decoratorCallback -= DecorateTweener;
            }
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            return child.UpdateNode(agent, blackboard);
        }

        protected abstract void DecorateTweener(Tweener tweener);
    }
}