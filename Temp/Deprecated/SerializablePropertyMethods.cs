using System;
namespace DefaultNamespace.MochiVariable
{
    [Serializable]
    public class SerializableGetter<T>: SerializableDelegate<Func<T>>{}
    
    [Serializable]
    public class SerializableSetter<T>:SerializableDelegate<Action<T>>{}
}