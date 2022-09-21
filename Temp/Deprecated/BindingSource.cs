using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
namespace DefaultNamespace.MochiVariable
{
    [Serializable]
    public class BindingSource<T> : BindingSource
    {
        public GameObject obj;
        [HideInInspector] public List<string> componentLabels;
        [HideInInspector] public List<Component> components;
        [HideInInspector] public Component selectedComponent;
        [HideInInspector] public int selectedComponentIndex;

        [HideInInspector] public List<string> properties = new();
        [HideInInspector] public int selectedPropertyIndex;
        [HideInInspector] public string selectedProperty;

        public Func<T> getValue;
        public Action<T> setValue;

        public object BoxedValue() => getValue is null ? null : getValue.Invoke();

        public void Refresh()
        {
            componentLabels.Clear();
            components.Clear();
            selectedComponentIndex = 0;
            selectedComponent = null;
            if (obj is null) return;
            try {
                foreach (var c in obj.GetComponents(typeof(Component))) {
                    var key = c.GetType().Name;
                    var occurence = 0;
                    while (componentLabels.Contains(key + " " + occurence)) {
                        occurence += 1;
                    }
                    componentLabels.Add(key + " " + occurence);
                    components.Add(c);
                }
            } catch {
                Debug.Log("Object Not Found");
                // ignored
            }
        }

        public void Reflect()
        {
            properties.Clear();
            selectedProperty = null;
            getValue = null;
            setValue = null;
            if (selectedComponentIndex >= 0 && selectedComponentIndex < components.Count)
                selectedComponent = components[selectedComponentIndex];
            if (selectedComponent is null) return;
            var propertyInfos = selectedComponent.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var propertyInfo in propertyInfos) {
                if (propertyInfo.PropertyType == typeof(T)) {
                    properties.Add(propertyInfo.Name);
                }
            }
            if (properties.Count > 0 && selectedPropertyIndex >= properties.Count) {
                selectedPropertyIndex = 0;
                selectedProperty = properties[0];
            }
            if(properties.Count>selectedPropertyIndex)
                selectedProperty = properties[selectedPropertyIndex];

        }

        public void Bind()
        {
            if (selectedProperty is null) return;
            if (selectedPropertyIndex >= 0 && selectedPropertyIndex < properties.Count)
                selectedProperty = properties[selectedPropertyIndex];
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

        public void ReEvaluate()
        {
            Component[] currentComps;
            try {
                //try to check if object is null or missing ref
                currentComps = obj.GetComponents(typeof(Component));
            } catch {
                WipeAll();
                return;
            }
            //The object is present, check if there is a change in component structure;

            var changeDetected = currentComps.Length != components.Count ||
                                 currentComps.Where((c, index) => c.GetInstanceID() != components[index].GetInstanceID()).Any();
            if (changeDetected) {
                components.Clear();
                componentLabels.Clear();
                foreach (var c in currentComps) {
                    var key = c.GetType().Name;
                    var occurence = 0;
                    while (componentLabels.Contains(key + " " + occurence)) {
                        occurence += 1;
                    }
                    componentLabels.Add(key + " " + occurence);
                    components.Add(c);
                }
            }
            var prevSelectedCompId = selectedComponent.GetInstanceID();
            var prevSelectedPersists = false;
            for (var index = 0; index < currentComps.Length; index++) {
                if (currentComps[index].GetInstanceID() != prevSelectedCompId) continue;
                selectedComponentIndex = index;
                prevSelectedPersists = true;
                break;
            }
            if (!prevSelectedPersists) {
                selectedComponent = null;
                selectedPropertyIndex = 0;
            }
            //Now check property modification
            if (selectedComponent is null) return;
            var propertyInfos = selectedComponent.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var propertyInfo in propertyInfos) {
                if (propertyInfo.PropertyType == typeof(T)) {
                    properties.Add(propertyInfo.Name);
                }
            }
            if (properties.Contains(selectedProperty)) selectedPropertyIndex = properties.IndexOf(selectedProperty);
            if (properties.Count > 0 && selectedPropertyIndex >= properties.Count) {
                selectedPropertyIndex = 0;
                selectedProperty = properties.Count < 1 ? null : properties[0];
            }
            Bind();
        }

        private void WipeAll()
        {
            obj = null;
            components.Clear();
            componentLabels.Clear();
            selectedComponent = null;
            selectedComponentIndex = 0;
            properties.Clear();
            selectedComponentIndex = 0;
            selectedProperty = null;
            getValue = null;
            setValue = null;
        }
    }
}