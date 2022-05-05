using UnityEngine;
namespace MochiBTS.Core.Primitives.Utilities.DataProcessors
{
    public class BaseMultiDataProcessor: ScriptableObject
    {
        public virtual object Process(object a, object b) { return a; }
    }
}