using System;
using MyonBTS.Core.Primitives.Utilities;
namespace MyonBTS.Core.Primitives.DataContainers
{
    [Serializable]
    public struct DataSource<T>
    {
        public SourceType sourceType;
        public string sourceName;
        public T value;

        public void ObtainValue(Agent agent,Blackboard blackboard)
        {
            value = sourceType switch {
                SourceType.BlackBoard => ReflectionUtil.GetValueFromBlackboard<T>(blackboard, sourceName),
                SourceType.Agent => ReflectionUtil.GetValueFromAgent<T>(agent, sourceName),
                _ => value
            };
        }
    }
}