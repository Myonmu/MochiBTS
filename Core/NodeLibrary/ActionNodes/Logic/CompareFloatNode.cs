using System;
using MochiBTS.Core.Primitives.Nodes;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.Logic
{
    public class CompareFloatNode: ComparatorNode<float>
    {

        protected override bool GreaterThan(float l, float r)
        {
            return l > r;
        }
        protected override bool EqualTo(float l, float r)
        {
            return Math.Abs(l - r) < 10e-4;
        }
    }
}