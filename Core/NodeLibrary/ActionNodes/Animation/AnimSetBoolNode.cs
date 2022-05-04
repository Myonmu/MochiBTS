using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.Animation
{
    public class AnimSetBoolNode: AnimatorSetNode<bool>
    {

        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            animator.SetBool(animParamName,parameter.value);
            return State.Success;
        }
    }
}