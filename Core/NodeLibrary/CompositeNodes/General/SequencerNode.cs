using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
namespace MochiBTS.Core.NodeLibrary.CompositeNodes.General
{
    public class SequencerNode : CompositeNode
    {
        private int currentChildIndex;
        public override string Tooltip =>
            "A sequencer node executes all children in left to right order. " +
            "Only moves to the next child if current child succeeds." +
            "Succeeds if all children succeed, fails if any child fails.";
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
                    currentChildIndex++;
                    break;
                case State.Failure:
                    return State.Failure;
                case State.Running:
                    return State.Running;
            }
            return currentChildIndex >= children.Count ? State.Success : State.Running;
        }
        
        public override void UpdateInfo()
        {
            if (children.Count < 1) {
                info = "No connected node";
                return;
            }
            info = $"{currentChildIndex.ToString()}~{children[currentChildIndex].name.Replace("Node","")}";
        }
    }
}