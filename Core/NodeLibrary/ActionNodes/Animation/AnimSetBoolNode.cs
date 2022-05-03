using MyonBTS.Core.Primitives.DataContainers;
using MyonBTS.Core.Primitives.Nodes;
namespace MyonBTS.Core.NodeLibrary.ActionNodes.Animation
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