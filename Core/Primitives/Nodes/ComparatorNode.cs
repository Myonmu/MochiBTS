using MyonBTS.Core.Primitives.DataContainers;
using MyonBTS.Core.Primitives.Utilities;
namespace MyonBTS.Core.Primitives.Nodes
{
    public abstract class ComparatorNode<T>: ActionNode
    {
        public override string tooltip =>
            "This node compares the left value and right value by the given compare mode." +
            "If the evaluation is true, it returns success, otherwise failure. " +
            "[This node uses reflection]";
        public CompareMode compareMode;
        public bool inclusive = false;
        public DataSource<T> left;
        public DataSource<T> right;
        protected override void OnStart(Agent agent, Blackboard blackboard)
        {
            left.ObtainValue(agent,blackboard);
            right.ObtainValue(agent,blackboard);
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
                    return inclusive && EqualTo(left.value, right.value )||
                            !GreaterThan(left.value, right.value) && !EqualTo(left.value, right.value)
                        ? State.Success : State.Failure;
            }
            return State.Running;
        }

        protected abstract bool GreaterThan(T l, T r);

        protected abstract bool EqualTo(T l, T r);
    }
}