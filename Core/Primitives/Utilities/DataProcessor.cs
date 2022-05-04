namespace MochiBTS.Core.Primitives.Utilities
{
    public abstract class DataProcessor<T> : BaseDataProcessor
    {
        public override void Process(object o)
        {
            if (o is T t) {
                OnProcess(t);
            }
        }

        protected abstract void OnProcess(T o);
    }
}