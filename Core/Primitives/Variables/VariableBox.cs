using System;
namespace MyonBTS.Core.Primitives.Variables
{
    public class VariableBox<T>: BaseVariable
    {

        public T value;
        public override Type type => typeof(T);
        public override object boxedValue { get=>value; set=>this.value=(T)value; }
    }
}