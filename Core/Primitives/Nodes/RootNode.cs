using MyonBTS.Core.Primitives.DataContainers;
using UnityEngine;
namespace MyonBTS.Core.Primitives.Nodes
{
    public class RootNode : Node
    {
        public override string tooltip =>
            "The evaluation of a behavior tree starts from here.";
        [HideInInspector]public Node child;

        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            return child.UpdateNode(agent,blackboard);
        }

        public override Node Clone()
        {
            var node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }
    }
}