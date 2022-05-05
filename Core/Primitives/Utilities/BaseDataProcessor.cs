using UnityEngine;
namespace MochiBTS.Core.Primitives.Utilities
{
    public class BaseDataProcessor: ScriptableObject
    {
        public virtual object Process(object o) { return o; }
    }
}