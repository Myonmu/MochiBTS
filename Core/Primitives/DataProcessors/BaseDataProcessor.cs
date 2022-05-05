using UnityEngine;
namespace MochiBTS.Core.Primitives.DataProcessors
{
    public class BaseDataProcessor : ScriptableObject
    {
        public virtual object Process(object o) { return o; }
    }
}