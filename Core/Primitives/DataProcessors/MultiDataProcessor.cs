using System;
using MochiBTS.Core.NodeLibrary.ActionNodes.Event;
namespace MochiBTS.Core.Primitives.DataProcessors
{
    public abstract class MultiDataProcessor<T> : BaseMultiDataProcessor
    {
        public override object Process(object a, object b)
        {
            if (a is T u && b is T v)
                return OnProcess(u, v);
            return a;
        }

        protected abstract T OnProcess(T u, T v);
    }
    
}