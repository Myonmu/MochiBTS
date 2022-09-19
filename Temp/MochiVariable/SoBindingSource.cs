using System;
using UnityEngine;
namespace DefaultNamespace.MochiVariable
{
    [Serializable]
    public class SoBindingSource<T>: BindingSource
    {
        public ScriptableObject obj;
        public string selectedProperty;
        public string selectedSub;

        public Func<T> getValue;
        public Action<T> setValue;
        

        public object BoxedValue()
        {
            try
            {
                return getValue is null ? null : getValue.Invoke();
            }
            catch
            {
                ResetDelegate();
            }

            return null;
        }

        public void ResetDelegate()
        {
            getValue = null;
            setValue = null;
        }

        public Type GetValueType()
        {
            return typeof(T);
        }

        public void Bind()
        {
            
            if (selectedProperty is null) return;
            var property = obj.GetType().GetProperty(selectedProperty);
            if (property != null)
            {
                var second = property.PropertyType.GetProperty(selectedSub);
                if (property.GetGetMethod() is not null && property.GetGetMethod().IsPublic)
                {
                    if (second is not null)
                    {
                        getValue = ReflectionUtils.CreateNestedGetter<T>(obj, property, second);
                    }
                    else
                    {
                        if (property.PropertyType != GetValueType()) return;
                        getValue = (Func<T>)property.GetGetMethod()
                            .CreateDelegate(typeof(Func<T>), obj);
                    }
                }

                if (property.GetSetMethod() is not null && property.GetSetMethod().IsPublic)
                {
                    if (second is not null)
                    {
                        setValue = ReflectionUtils.CreateNestedSetter<T>(obj, property, second);
                    }
                    else
                    {
                        if (property.PropertyType != GetValueType()) return;
                        setValue = (Action<T>)property.GetSetMethod()
                            .CreateDelegate(typeof(Action<T>), obj);
                    }
                }
            }
            else
            {
                var field = obj.GetType().GetField(selectedProperty);
                if (field is null) return;
                var second = field.FieldType.GetProperty(selectedSub);
                if (second is not null)
                {
                    getValue = ReflectionUtils.CreateNestedGetter<T>(obj, field, second);
                    setValue = ReflectionUtils.CreateNestedSetter<T>(obj, field, second);
                }
                else
                {
                    if (field.FieldType != GetValueType()) return;
                    getValue = ReflectionUtils.CreateGetter<T>(obj, field);
                    setValue = ReflectionUtils.CreateSetter<T>(obj, field);
                }
            }
        }
    }
}