using System;
using MochiBTS.Core.Primitives.MochiVariable;
using MochiBTS.Core.Primitives.Utilities;
namespace MochiBTS.Core.Primitives.DataContainers
{
    [Serializable]
    public struct DataSource<T>
    {
        public SourceType sourceType;
        public string sourceName;
        public T value;
        private Action<T> setter;
        private Func<T> getter;

        public void GetValue(Agent agent, Blackboard blackboard)
        {
            InitializeBindings(agent, blackboard);
            value = sourceType switch {
                SourceType.BlackBoard => getter.Invoke(),
                SourceType.Agent => getter.Invoke(),
                SourceType.VariableBoard => agent.variableBoard.GetValue<T>(sourceName),
                _ => value
            };
        }

        public void SetValue(T val, Agent agent, Blackboard blackboard)
        {
            switch (sourceType) {
                case SourceType.BlackBoard:
                    setter.Invoke(val);
                    break;
                case SourceType.Agent:
                    setter.Invoke(val);
                    break;
                case SourceType.VariableBoard:
                    agent.variableBoard.SetValue(sourceName, val);
                    break;
                case SourceType.None:
                    value = val;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void InitializeBindings(Agent agent, Blackboard blackboard)
        {
            switch (sourceType)
            {
                case SourceType.BlackBoard:
                    getter ??= ReflectionUtils.CreateGetter<T>(blackboard, sourceName);
                    setter ??= ReflectionUtils.CreateSetter<T>(blackboard, sourceName);
                    break;
                case SourceType.Agent:
                    getter ??= ReflectionUtils.CreateGetter<T>(agent, sourceName);
                    setter ??= ReflectionUtils.CreateSetter<T>(agent, sourceName);
                    break;
                case SourceType.VariableBoard:
                    break;
                case SourceType.None:
                    break;
            }
        }
    }
}