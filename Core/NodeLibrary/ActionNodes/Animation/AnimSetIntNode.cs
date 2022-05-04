using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.Animation
{
    public class AnimSetIntNode: AnimatorSetNode<int>
    {

        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            animator.SetInteger(animParamName,parameter.value);
            return State.Success;
        }
    }
}