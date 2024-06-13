using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
namespace MochiBTS.Core.NodeLibrary.DecoratorNodes.General
{
    public class SkipNode : DecoratorNode
    {

        public bool enableSkip = false;
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
            
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            if (enableSkip) return State.Success;
            return child.UpdateNode(agent, blackboard);
        }

        public override void UpdateInfo()
        {
            info = enableSkip ? "Skipping" : "";
        }
    }
}