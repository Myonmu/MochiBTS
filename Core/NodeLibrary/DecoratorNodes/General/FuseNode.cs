using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
namespace MochiBTS.Core.NodeLibrary.DecoratorNodes.General
{
    public class FuseNode : DecoratorNode
    {
        public State outputState = State.Success;
        public State detectState = State.Success;
        public bool notEqualTo;
        private bool detected;
        public override string Tooltip =>
            "Will keep running until child node returns a state equal to detectState " +
            "(or not equal to if notEqualTo is set to true). " +
            "The node will return outputState if the previous condition is true. After that, " +
            "any visit to this node will immediately return outputState without executing the child.";
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
            if (!notEqualTo && childState != detectState ||
                notEqualTo && childState == detectState) return State.Running;
            detected = true;
            return outputState;
        }

        public override void UpdateInfo()
        {
            var s = detected ? "Broken" : "Passing";
            subInfo = $"Break on: {detectState}";
            info = $"({s})";
        }
    }
}