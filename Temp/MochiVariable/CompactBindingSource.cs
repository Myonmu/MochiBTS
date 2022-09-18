using System;
using UnityEngine;
namespace DefaultNamespace.MochiVariable
{
    [Serializable]
    public class CompactBindingSource<T>: IBindingSource
    {
        public GameObject obj;
        [HideInInspector] public Component selectedComponent;
        [HideInInspector] public string selectedProperty;

        public Func<T> getValue;
        public Action<T> setValue;

        public object BoxedValue()
        {
            try {
                return getValue is null ? null : getValue.Invoke();
            } catch {
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
            var field = selectedComponent.GetType().GetProperty(selectedProperty);
            if (field is not null) {
                if (field.GetGetMethod() is not null && field.GetGetMethod().IsPublic) {
                    getValue = (Func<T>)field.GetGetMethod().CreateDelegate(typeof(Func<T>), selectedComponent);
                }
                if (field.GetSetMethod() is not null && field.GetSetMethod().IsPublic) {
                    setValue = (Action<T>)field.GetSetMethod().CreateDelegate(typeof(Action<T>), selectedComponent);
                }
            }
        }
    }
}