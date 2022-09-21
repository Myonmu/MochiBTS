using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
namespace MochiBTS.Core.NodeLibrary.DecoratorNodes.General
{
    public class LoopNode : DecoratorNode
    {

        public bool infiniteLoop;
        public int iterations;
        private int loopCounter;
        public override string Tooltip =>
            "Keeps the connected node running. If infiniteLoop is disabled, " +
            "will succeed when the number of iterations is reached.";
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            loopCounter = iterations;
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {

        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            if (child.UpdateNode(agent, blackboard) is State.Success)
                loopCounter--;
            return infiniteLoop ? State.Running : loopCounter <= 0 ? State.Success : State.Running;
        }
    }
}