using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
namespace MochiBTS.Core.NodeLibrary.ActionNodes.Varialble
{
    public class SetVariableNode<T>: ActionNode
    {

        public DataSource<T> variable;
        public T targetValue;
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            variable.SetValue(targetValue,agent,blackboard);
        }
        protected override void OnStop(Agent agent, Blackboard blackboard)
        {
            
        }
        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            return State.Success;
        }

        public override void UpdateInfo()
        {
            subInfo = $"{variable.sourceName} => {targetValue}";
        }
    }
}