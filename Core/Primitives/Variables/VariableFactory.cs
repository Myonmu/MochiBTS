using System;
using UnityEngine.Serialization;
namespace MyonBTS.Core.Primitives.Variables
{
    [Serializable]
    public class VariableFactory
    {
        public enum VariableType
        {
            Bool,Integer,Float,Vector3
        }
        public VariableType type;

        public BoolVariable BoolVariable = new();
        public IntegerVariable IntegerVariable = new();
        public FloatVariable FloatVariable = new();
        public Vector3Variable Vector3Variable = new();

        public BaseVariable CreateVariable()
        {
            return GetVariableFromType(type);
        }
        private BaseVariable GetVariableFromType(VariableType variableType)
        {
            switch (variableType) {

                case VariableType.Bool:
                    return BoolVariable;
                case VariableType.Integer:
                    return IntegerVariable;
                case VariableType.Float:
                    return FloatVariable;
                case VariableType.Vector3:
                    return Vector3Variable;
                default:
                    throw new ArgumentOutOfRangeException(nameof(variableType), variableType, null);
            }
        }


        public Type GetVariableType(VariableType varFactoryType)
        {
            return GetVariableFromType(type).GetType();
        }
    }
}