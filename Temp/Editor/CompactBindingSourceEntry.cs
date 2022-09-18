﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace DefaultNamespace.Editor
{
/* Since all binding source share the same property drawer, we keep a registry of all the instances
          * to avoid using reflections for initialization. (trade memory for performance)
          */
    [Serializable]
    public class CompactBindingSourceEntry
    {
        public int id;
        public object propertyObject;
        public Type objectType;
        public Action bind;
        public Action resetDelegates;
        public Func<Type> getValueType;
        public Func<object> getValue;
        public List<Component> components = new();
        public List<string> componentNames = new();
        public int selectedComponentIndex;
        public List<string> properties = new();
        public int selectedPropertyIndex;

        public void Refresh(SerializedProperty prop)
        {
            componentNames.Clear();
            components.Clear();
            selectedComponentIndex = 0;
            prop.FindPropertyRelative("selectedComponent").boxedValue = null;
            prop.serializedObject.ApplyModifiedProperties();
            prop.serializedObject.Update();
            var obj = (GameObject)prop.FindPropertyRelative("obj").boxedValue;
            if (obj is null) return;
            try {
                foreach (var c in obj.GetComponents(typeof(Component))) {
                    var key = c.GetType().Name;
                    var occurence = 0;
                    while (componentNames.Contains(key + " " + occurence)) {
                        occurence += 1;
                    }
                    componentNames.Add(key + " " + occurence);
                    components.Add(c);
                }
            } catch {
                Debug.Log("Object Not Found");
                // ignored
            }
        }

        public void Reflect(SerializedProperty prop)
        {
            properties.Clear();
            prop.FindPropertyRelative("selectedProperty").stringValue = null;
            prop.serializedObject.ApplyModifiedProperties();
            prop.serializedObject.Update();
            resetDelegates?.Invoke();
            if (selectedComponentIndex >= 0 && selectedComponentIndex < components.Count)
                prop.FindPropertyRelative("selectedComponent").boxedValue = components[selectedComponentIndex];
            prop.serializedObject.ApplyModifiedProperties();
            prop.serializedObject.Update();
            var selectedComponent = prop.FindPropertyRelative("selectedComponent").boxedValue;
            if (selectedComponent is null) return;
            var propertyInfos = selectedComponent.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var propertyInfo in propertyInfos) {
                if (propertyInfo.PropertyType == getValueType.Invoke()) {
                    properties.Add(propertyInfo.Name);
                }
            }
            if (properties.Count > 0 && selectedPropertyIndex >= properties.Count) {
                selectedPropertyIndex = 0;
                prop.FindPropertyRelative("selectedProperty").stringValue = properties[0];
                prop.serializedObject.ApplyModifiedProperties();
                prop.serializedObject.Update();
            }
            if (properties.Count > selectedPropertyIndex) {
                prop.FindPropertyRelative("selectedProperty").stringValue = properties[selectedPropertyIndex];
                prop.serializedObject.ApplyModifiedProperties();
                prop.serializedObject.Update();
                
            }
        }

        public bool ReEvaluate(SerializedProperty prop)
        {
            Component[] currentComps;
            try {
                //try to check if object is null or missing ref
                currentComps = ((GameObject)prop.FindPropertyRelative("obj").boxedValue).GetComponents(typeof(Component));
            } catch {
                WipeAll(prop);
                return false;
            }
            //The object is present, check if there is a change in component structure;

            var changeDetected = currentComps.Length != components.Count ||
                                 currentComps.Where((c, index) => c.GetInstanceID() != components[index].GetInstanceID()).Any();
            if (changeDetected) {
                components.Clear();
                componentNames.Clear();
                foreach (var c in currentComps) {
                    var key = c.GetType().Name;
                    var occurence = 0;
                    while (componentNames.Contains(key + " " + occurence)) {
                        occurence += 1;
                    }
                    componentNames.Add(key + " " + occurence);
                    components.Add(c);
                }
            }
            var prevSelectedCompId = ((Component)prop.FindPropertyRelative("selectedComponent").boxedValue).GetInstanceID();
            var prevSelectedPersists = false;
            for (var index = 0; index < currentComps.Length; index++) {
                if (currentComps[index].GetInstanceID() != prevSelectedCompId) continue;
                selectedComponentIndex = index;
                prevSelectedPersists = true;
                break;
            }
            if (!prevSelectedPersists) {
                prop.FindPropertyRelative("selectedComponent").boxedValue = null;
                selectedPropertyIndex = 0;
            }
            //Now check property modification
            if (prop.FindPropertyRelative("selectedComponent").boxedValue is null) return false;
            var propertyInfos = prop.FindPropertyRelative("selectedComponent").boxedValue.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var propertyInfo in propertyInfos) {
                if (propertyInfo.PropertyType == getValueType.Invoke()) {
                    properties.Add(propertyInfo.Name);
                }
            }
            var selectedProperty = prop.FindPropertyRelative("selectedProperty").stringValue;
            if (properties.Contains(selectedProperty)) selectedPropertyIndex = properties.IndexOf(selectedProperty);
            if (properties.Count > 0 && selectedPropertyIndex >= properties.Count) {
                selectedPropertyIndex = 0;
                prop.FindPropertyRelative("selectedProperty").stringValue = properties.Count < 1 ? null : properties[0];
                prop.serializedObject.ApplyModifiedProperties();
                prop.serializedObject.Update();
            }
            UpdateSelected(prop);
            bind.Invoke();
            return true;
        }

        public void UpdateSelected(SerializedProperty prop)
        {
            if (properties.Count > selectedPropertyIndex)
                prop.FindPropertyRelative("selectedProperty").stringValue = properties[selectedPropertyIndex];
            prop.serializedObject.ApplyModifiedProperties();
            prop.serializedObject.Update();
        }

        private void WipeAll(SerializedProperty prop)
        {
            prop.FindPropertyRelative("obj").boxedValue = null;
            prop.serializedObject.ApplyModifiedProperties();
            prop.serializedObject.Update();
            components.Clear();
            componentNames.Clear();
            prop.FindPropertyRelative("selectedComponent").boxedValue = null;
            prop.serializedObject.ApplyModifiedProperties();
            prop.serializedObject.Update();
            selectedComponentIndex = 0;
            properties.Clear();
            selectedComponentIndex = 0;
            prop.FindPropertyRelative("selectedProperty").stringValue = null;
            prop.serializedObject.ApplyModifiedProperties();
            prop.serializedObject.Update();
            resetDelegates.Invoke();

        }
    }
}