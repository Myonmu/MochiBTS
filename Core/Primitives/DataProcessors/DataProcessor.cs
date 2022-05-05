namespace MochiBTS.Core.Primitives.DataProcessors
{
    public abstract class DataProcessor<T> : BaseDataProcessor
    {
        public override object Process(object o)
        {
            if (o is T t)
                return OnProcess(t);
            return o;
        }

        protected abstract T OnProcess(T o);
    }
}