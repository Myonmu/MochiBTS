using MyonBTS.Core.Primitives.DataContainers;
using MyonBTS.Core.Primitives.Nodes;
namespace MyonBTS.Core.NodeLibrary.DecoratorNodes.General
{
    public class ForceFailureNode : DecoratorNode
    {

        public override string tooltip =>
            "Returns Failure regardless of what the child returns";
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {

        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {

        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            child.UpdateNode(agent,blackboard);
            return State.Failure;
        }
    }
}