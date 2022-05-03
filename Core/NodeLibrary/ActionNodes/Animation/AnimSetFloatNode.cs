using System;
using System.Reflection;
using MyonBTS.Core.Primitives.DataContainers;
using MyonBTS.Core.Primitives.Nodes;
using MyonBTS.Core.Primitives.Utilities;
using UnityEngine;
namespace MyonBTS.Core.NodeLibrary.ActionNodes.Animation
{
    public class AnimSetFloatNode: AnimatorSetNode<float>
    {
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            animator.SetFloat(animParamName,parameter.value);
            return State.Success;
        }
    }
}