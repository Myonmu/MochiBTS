using MyonBTS.Core.Primitives.DataContainers;
using Blackboard = MyonBTS.Core.Primitives.DataContainers.Blackboard;
namespace MyonBTS.Core.Primitives.Utilities
{
    public class ReflectionUtil
    {
        public static T GetFieldValue<T>(object o, string fieldName)
        {
            var field = o.GetType().GetField(fieldName);
            return (T)field.GetValue(o);
        }

        public static void SetFieldValue<T>(object o, string fieldName, T value)
        {
            var field = o.GetType().GetField(fieldName);
            field.SetValue(o,value);
        }
        
        //Same as the first method but reduces 1 boxing-unboxing
        public static T GetValueFromBlackboard<T>(Blackboard b, string fieldName)
        {
            return (T)b.GetType().GetField(fieldName).GetValue(b);
        }
        
        public static T GetValueFromAgent<T>(Agent a, string fieldName)
        {
            return (T)a.GetType().GetField(fieldName).GetValue(a);
        }
    }
}