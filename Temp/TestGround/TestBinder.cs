using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
namespace DefaultNamespace.TestGround
{
    [Serializable]
    public class TestBinder: MonoBehaviour
    {
        public GameObject obj;
        
        [HideInInspector] public List<string> componentLabels;
        [HideInInspector] public List<Component> components;
        [HideInInspector] public Component selectedComponent;
        [HideInInspector] public int selectedComponentIndex;
        
        [HideInInspector] public List<string> properties = new();
        [HideInInspector] public int selectedPropertyIndex;
        [HideInInspector] public string selectedProperty;
        
        public Vector3 value;

        //public string fieldName;

        public Func<Vector3> getter;
        public Action<Vector3> setter;

        public void Refresh()
        {
            componentLabels.Clear();
            components.Clear();
            selectedComponentIndex = 0;
            if (obj is null) return;
            foreach (var c in obj.GetComponents(typeof(Component)) ) {
                var key = c.GetType().Name;
                var occurence = 0;
                while (componentLabels.Contains(key + " " + occurence)) {
                    occurence += 1;
                }
                componentLabels.Add(key + " "+ occurence);
                components.Add(c);
            }
        }

        public void Reflect()
        {
            properties.Clear();
            if (selectedComponent is null) return;

            var propertyInfos = selectedComponent.GetType().
                GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var propertyInfo in propertyInfos) {
                if (propertyInfo.PropertyType == typeof(Vector3)) {
                    properties.Add(propertyInfo.Name);
                }
            }
        }

        public void Bind()
        {
            var field = selectedComponent.GetType().GetProperty(selectedProperty);
            if ( field is not null) {
                if (field.GetGetMethod() is not null && field.GetGetMethod().IsPublic) {
                    getter = (Func<Vector3>)field.GetGetMethod().CreateDelegate(typeof(Func<Vector3>), selectedComponent);
                }
                if (field.GetSetMethod() is not null && field.GetSetMethod().IsPublic) {
                    setter = (Action<Vector3>)field.GetSetMethod().CreateDelegate(typeof(Action<Vector3>), selectedComponent);
                }
            }
        }
    }
    
}