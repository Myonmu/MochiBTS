using DG.Tweening;
using MochiBTS.Extension.Primitives;
namespace MochiBTS.Extension.NodeLibrary.DecoratorNodes.Tween
{
    public class LoopTweenNode: TweenDecoratorNode
    {
        public override string tooltip =>
            "Sets the connected tweener action node to loop.";
        public int loopCount;
        public LoopType loopType;
        protected override void DecorateTweener(Tweener tweener)
        {
            tweener.SetLoops(loopCount, loopType);
        }
    }
}