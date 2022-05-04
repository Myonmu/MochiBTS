using UnityEngine;
namespace MochiBTS.Core.Primitives.Utilities
{
    public class BaseDataProcessor: ScriptableObject
    {
        public virtual void Process(object o) {}
    }
}