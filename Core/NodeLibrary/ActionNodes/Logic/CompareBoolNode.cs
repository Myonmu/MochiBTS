using MochiBTS.Core.Primitives.Nodes;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.Logic
{
    public class CompareBoolNode: ComparatorNode<bool>
    {

        protected override bool GreaterThan(bool l, bool r)
        {
            return !(l && r);
        }
        protected override bool EqualTo(bool l, bool r)
        {
            return l && r;
        }
    }
}