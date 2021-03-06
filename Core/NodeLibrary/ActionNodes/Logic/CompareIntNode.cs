using MochiBTS.Core.Primitives.Nodes;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.Logic
{
    public class CompareIntNode : ComparatorNode<int>
    {
        protected override bool GreaterThan(int l, int r)
        {
            return l > r;
        }
        protected override bool EqualTo(int l, int r)
        {
            return l == r;
        }
    }
}