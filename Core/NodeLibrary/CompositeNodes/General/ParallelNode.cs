using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
namespace MochiBTS.Core.NodeLibrary.CompositeNodes.General
{
    public class ParallelNode : CompositeNode
    {
        public int successThreshold;

        public override string tooltip =>
            "A parallel node executes all children in left to right order in the same tick. " +
            "Succeeds if number of succeeding children reaches successThreshold, fails if all children fail." +
            "Keeps running if at least one child is running.";
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            if (successThreshold == 0)
                successThreshold = children.Count;
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {

        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            var successfulNodes = 0;
            var hasChildrenRunning = false;
            foreach (var node in children) {
                switch (node.UpdateNode(agent, blackboard)) {
                    case State.Success:
                        successfulNodes += 1;
                        break;
                    case State.Running:
                        hasChildrenRunning = true;
                        break;
                }
                if (successfulNodes >= successThreshold) return State.Success;
            }
            return hasChildrenRunning ? State.Running : State.Failure;
        }
    }
}