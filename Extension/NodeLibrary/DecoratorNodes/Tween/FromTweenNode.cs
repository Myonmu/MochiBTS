using DG.Tweening;
using MochiBTS.Extension.Primitives;
namespace MochiBTS.Extension.NodeLibrary.DecoratorNodes.Tween
{
    public class FromTweenNode : TweenDecoratorNode
    {
        public override string Tooltip =>
            "Sets the connected tweener action node to From";
        protected override void DecorateTweener(Tweener tweener)
        {
            tweener.From();
        }
    }
}