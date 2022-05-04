using MyonBTS.Core.Primitives.DataContainers;
using UnityEngine;
namespace MyonBTS.Core.Primitives.Utilities
{
    public class BaseDataProcessor: ScriptableObject
    {
        public virtual void Process(object o) {}
    }
}