using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
namespace DefaultNamespace.MochiVariable
{
    [Serializable]
    public class SerializableDelegate<T> where T : class
    {
        [SerializeField] private byte[] serializedData = {};

        static SerializableDelegate()
        {
            if (!typeof(T).IsSubclassOf(typeof(Delegate))) {
                throw new InvalidOperationException($"{typeof(T).Name} is not a delegate");
            }
        }

        public void SetDelegate(T action)
        {
            if (action is null) {
                serializedData = new byte[] {
                };
                return;
            }

            if (action is not Delegate) {
                throw new InvalidOperationException($"{typeof(T).Name} is not a delegate");
            }

            using var stream = new MemoryStream();
            new BinaryFormatter().Serialize(stream,action);
            stream.Flush();
            serializedData = stream.ToArray();

        }

        public T CreateDelegate()
        {
            using var stream = new MemoryStream(serializedData);
            return new BinaryFormatter().Deserialize(stream) as T;
        }
    }
}