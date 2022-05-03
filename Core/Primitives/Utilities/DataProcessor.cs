using MyonBTS.Core.Primitives.DataContainers;
namespace MyonBTS.Core.Primitives.Utilities
{
    public abstract class DataProcessor<T> : BaseDataProcessor
    {
        public override void Process(Agent agent)
        {
            if (agent is T t) {
                OnProcess(t);
            }
        }

        protected abstract void OnProcess(T agent);
    }
}