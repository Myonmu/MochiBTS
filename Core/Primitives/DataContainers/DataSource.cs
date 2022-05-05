using System;
using MochiBTS.Core.Primitives.Utilities;
namespace MochiBTS.Core.Primitives.DataContainers
{
    [Serializable]
    public struct DataSource<T>
    {
        public SourceType sourceType;
        public string sourceName;
        public T value;

        public void GetValue(Agent agent,Blackboard blackboard)
        {
            value = sourceType switch {
                SourceType.BlackBoard => ReflectionUtil.GetValueFromBlackboard<T>(blackboard, sourceName),
                SourceType.Agent => ReflectionUtil.GetValueFromAgent<T>(agent, sourceName),
                SourceType.VariableBoard => agent.variableBoard.GetValue<T>(sourceName),
                _ => value
            };
        }

        public void SetValue(T val, Agent agent, Blackboard blackboard)
        {
            switch (sourceType) {
                case SourceType.BlackBoard:
                    ReflectionUtil.SetFieldValue(blackboard,sourceName,val);
                    break;
                case SourceType.Agent:
                    ReflectionUtil.SetFieldValue(agent,sourceName,val);
                    break;
                case SourceType.VariableBoard:
                    agent.variableBoard.SetValue<T>(sourceName,val);
                    break;
                case SourceType.None:
                    value = val;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}