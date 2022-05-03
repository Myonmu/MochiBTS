using MyonBTS.Core.Primitives.DataContainers;
using MyonBTS.Core.Primitives.Nodes;
namespace MyonBTS.Core.NodeLibrary.ActionNodes.Animation
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