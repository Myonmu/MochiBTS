using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Utilities;
namespace MochiBTS.Core.Primitives.Nodes
{
    public abstract class ComparatorNode<T> : ActionNode
    {
        public CompareMode compareMode;
        public bool inclusive;
        public DataSource<T> left;
        public DataSource<T> right;
        public override string Tooltip =>
            "This node compares the left value and right value by the given compare mode." +
            "If the evaluation is true, it returns success, otherwise failure. " +
            "[This node uses reflection]";
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            left.GetValue(agent, blackboard);
            right.GetValue(agent, blackboard);
        }

        protected override void OnStop(Agent agent, Blackboard blackboard)
        {

        }

        protected override State OnUpdate(Agent agent, Blackboard blackboard)
        {
            switch (compareMode) {
                case CompareMode.EqualTo:
                    return EqualTo(left.value, right.value) ? State.Success : State.Failure;
                case CompareMode.GreaterThan:
                    return inclusive && EqualTo(left.value, right.value) || GreaterThan(left.value, right.value)
                        ? State.Success : State.Failure;
                case CompareMode.LessThan:
                    return inclusive && EqualTo(left.value, right.value) ||
                           !GreaterThan(left.value, right.value) && !EqualTo(left.value, right.value)
                        ? State.Success : State.Failure;
            }
            return State.Running;
        }

        protected abstract bool GreaterThan(T l, T r);

        protected abstract bool EqualTo(T l, T r);
    }
}