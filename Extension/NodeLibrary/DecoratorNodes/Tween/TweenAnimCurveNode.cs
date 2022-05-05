using DG.Tweening;
using MochiBTS.Extension.Primitives;
using UnityEngine;
namespace MochiBTS.Extension.NodeLibrary.DecoratorNodes.Tween
{
    public class TweenAnimCurveNode : TweenDecoratorNode
    {
        public AnimationCurve animationCurve;

        public override string tooltip =>
            "Sets the ease of the connected tweener action node to an animation curve.";
        protected override void DecorateTweener(Tweener tweener)
        {
            tweener.SetEase(animationCurve);
        }
    }
}