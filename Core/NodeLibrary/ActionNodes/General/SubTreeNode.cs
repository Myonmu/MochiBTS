using MyonBTS.Core.Primitives;
using MyonBTS.Core.Primitives.DataContainers;
using MyonBTS.Core.Primitives.Nodes;
using UnityEngine;
namespace MyonBTS.Core.NodeLibrary.ActionNodes.General
{
    public class SubTreeNode : ActionNode
    {
        public BehaviorTree subTree;
        public bool inheritBlackboard = true;
        public bool forceExecuteTree = false;
        public override string tooltip =>
            "Execute the behavior tree attached to this node. Will create a copy of the attached tree." +
            "Assigning the same tree that contains this node will cause explosion, NEVER DO THAT!";

        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            if (subTree.blackboard is not null) return;
            subTree.blackboard = blackboard;
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {

        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            if (subTree is not null) {
                var treeState = subTree.UpdateTree(agent,forceExecute:forceExecuteTree);
                return treeState;
            }
            Debug.LogError("SubTree node has no tree assigned!");
            return State.Failure;
        }

        // public override void Bind(Blackboard b, Agent a)
        // {
        //     base.Bind(b,a);
        //     if (overwriteBlackboard) subTree.blackboard = blackboard;
        //     subTree.Bind(a);
        // }
        public override Node Clone()
        {
            var cloned = Instantiate(this);
            var treeClone = subTree?.Clone();
            cloned.subTree = treeClone;
            return cloned;
        }
    }
}