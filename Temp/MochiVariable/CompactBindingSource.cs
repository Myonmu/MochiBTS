using System;
using UnityEngine;

namespace DefaultNamespace.MochiVariable
{
    [Serializable]
    public class CompactBindingSource<T> : IBindingSource
    {
        public GameObject obj;
        public Component selectedComponent;
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
            var property = selectedComponent.GetType().GetProperty(selectedProperty);
            if (property != null)
            {
                var second = property.PropertyType.GetProperty(selectedSub);
                if (property.GetGetMethod() is not null && property.GetGetMethod().IsPublic)
                {
                    if (second is not null)
                    {
                        getValue = ReflectionUtils.CreateNestedGetter<T>(selectedComponent, property, second);
                    }
                    else
                    {
                        if (property.PropertyType != GetValueType()) return;
                        getValue = (Func<T>)property.GetGetMethod()
                            .CreateDelegate(typeof(Func<T>), selectedComponent);
                    }
                }

                if (property.GetSetMethod() is not null && property.GetSetMethod().IsPublic)
                {
                    if (second is not null)
                    {
                        setValue = ReflectionUtils.CreateNestedSetter<T>(selectedComponent, property, second);
                    }
                    else
                    {
                        if (property.PropertyType != GetValueType()) return;
                        setValue = (Action<T>)property.GetSetMethod()
                            .CreateDelegate(typeof(Action<T>), selectedComponent);
                    }
                }
            }
            else
            {
                var field = selectedComponent.GetType().GetField(selectedProperty);
                if (field is null) return;
                var second = field.FieldType.GetProperty(selectedSub);
                if (second is not null)
                {
                    getValue = ReflectionUtils.CreateNestedGetter<T>(selectedComponent, field, second);
                    setValue = ReflectionUtils.CreateNestedSetter<T>(selectedComponent, field, second);
                }
                else
                {
                    if (field.FieldType != GetValueType()) return;
                    getValue = ReflectionUtils.CreateGetter<T>(selectedComponent, field);
                    setValue = ReflectionUtils.CreateSetter<T>(selectedComponent, field);
                }
            }
        }
    }
}