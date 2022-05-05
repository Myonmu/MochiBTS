using System;
using MochiBTS.Core.Primitives.Variables.Builtin;
namespace MochiBTS.Core.Primitives.Variables
{
    [Serializable]
    public class VariableFactory
    {
        // ======1====== Add an entry in the enum
        public enum VariableType
        {
            Bool,
            Integer,
            Float,
            Vector3,
            Transform
        }
        public VariableType type;

        // ======2====== Add a public field of the corresponding type
        // !!!!! Field name and field type must be the same!
        public BoolVariable BoolVariable = new();
        public IntegerVariable IntegerVariable = new();
        public FloatVariable FloatVariable = new();
        public Vector3Variable Vector3Variable = new();
        public TransformVariable TransformVariable = new();


        // ======3====== Add a new case to return the field.
        private BaseVariable GetVariableFromType(VariableType variableType)
        {
            switch (variableType) {
                case VariableType.Bool: return BoolVariable;
                case VariableType.Integer: return IntegerVariable;
                case VariableType.Float: return FloatVariable;
                case VariableType.Vector3: return Vector3Variable;
                case VariableType.Transform: return TransformVariable;
                default:
                    throw new ArgumentOutOfRangeException(nameof(variableType), variableType, null);
            }
        }

        public BaseVariable CreateVariable()
        {
            return GetVariableFromType(type);
        }

        public Type GetVariableType()
        {
            return GetVariableFromType(type).GetType();
        }
    }
}