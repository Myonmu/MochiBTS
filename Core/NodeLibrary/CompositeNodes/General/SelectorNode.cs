using MyonBTS.Core.Primitives.DataContainers;
using MyonBTS.Core.Primitives.Nodes;
namespace MyonBTS.Core.NodeLibrary.CompositeNodes
{
    public class SelectorNode : CompositeNode
    {
        private int currentChildIndex;
        public override string tooltip =>
            "A selector node executes all children in left to right order. " +
            "Only moves to the next child if current child fails." +
            "Succeeds if any child succeeds, fails if all children fail.";
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            currentChildIndex = 0;
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {

        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            var child = children[currentChildIndex];
            switch (child.UpdateNode(agent, blackboard)) {
                case State.Success:
                    return State.Success;
                case State.Failure:
                    currentChildIndex++;
                    break;
                case State.Running:
                    return State.Running;
            }
            return currentChildIndex >= children.Count ? State.Failure : State.Running;
        }
    }
}