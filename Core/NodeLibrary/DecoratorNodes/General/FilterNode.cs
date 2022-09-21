using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
namespace MochiBTS.Core.NodeLibrary.DecoratorNodes.General
{
    public class FilterNode : DecoratorNode
    {
        public State passingState;
        public bool notEqualTo;
        public override string Tooltip =>
            "Always executes its child. Returns Running unless the child returns a state equal to passingState." +
            "Returns passingState if it is the case.";
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {

        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            var childState = child.UpdateNode(agent, blackboard);
            if (!notEqualTo && childState != passingState ||
                notEqualTo && childState == passingState) return State.Running;
            return passingState;
        }
    }
}