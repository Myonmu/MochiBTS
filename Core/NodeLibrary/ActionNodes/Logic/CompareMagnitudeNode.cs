using System;
using MochiBTS.Core.Primitives.Nodes;
using UnityEngine;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.Logic
{
    public class CompareMagnitudeNode: ComparatorNode<Vector3>
    {

        protected override bool GreaterThan(Vector3 l, Vector3 r)
        {
            return l.magnitude > r.magnitude;
        }
        protected override bool EqualTo(Vector3 l, Vector3 r)
        {
            return Math.Abs(l.magnitude - r.magnitude) < 1e-4;
        }
    }
}