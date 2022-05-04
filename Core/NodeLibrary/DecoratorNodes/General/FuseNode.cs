using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
namespace MochiBTS.Core.NodeLibrary.DecoratorNodes.General
{
    public class FuseNode:DecoratorNode
    {
        public override string tooltip =>
        "Will keep running until child node returns a state equal to detectState " +
        "(or not equal to if notEqualTo is set to true). " +
        "The node will return outputState if the previous condition is true. After that, " +
        "any visit to this node will immediately return outputState without executing the child."; 
        public Node.State outputState;
        public Node.State detectState;
        public bool notEqualTo = false;
        private bool detected = false;
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            if (detected) return outputState;
            var childState = child.UpdateNode(agent, blackboard);
            if (!notEqualTo&& childState != detectState||
                notEqualTo&&childState==detectState) return State.Running;
            detected = true;
            return outputState;
        }
    }
}