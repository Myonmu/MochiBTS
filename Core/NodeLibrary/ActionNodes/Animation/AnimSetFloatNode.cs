using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.Animation
{
    public class AnimSetFloatNode : AnimatorSetNode<float>
    {
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            animator.SetFloat(animParamName, parameter.value);
            return State.Success;
        }
    }
}