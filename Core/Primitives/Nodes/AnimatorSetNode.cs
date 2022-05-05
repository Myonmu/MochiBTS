using MochiBTS.Core.Primitives.DataContainers;
using UnityEngine;
namespace MochiBTS.Core.Primitives.Nodes
{
    public abstract class AnimatorSetNode<T>: ActionNode
    {
        public override string tooltip => "This node sets the animator parameter attached to the agent by the given value," +
                                          "or the value read from an accessible field from the agent or blackboard." +
                                          "Returns Success after set. [This node uses reflection] .";
        public string animParamName;
        public DataSource<T> parameter;
        protected Animator animator;

        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            animator ??= agent.GetComponent<Animator>();
            parameter.GetValue(agent,blackboard);
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {

        }
    }
}