using DG.Tweening;
using MochiBTS.Extension.Primitives;
namespace MochiBTS.Extension.NodeLibrary.DecoratorNodes.Tween
{
    public class LoopTweenNode : TweenDecoratorNode
    {
        public int loopCount;
        public LoopType loopType;
        public override string tooltip =>
            "Sets the connected tweener action node to loop.";
        protected override void DecorateTweener(Tweener tweener)
        {
            tweener.SetLoops(loopCount, loopType);
        }
    }
}