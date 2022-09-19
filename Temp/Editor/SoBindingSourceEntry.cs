using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace DefaultNamespace.Editor
{
    [Serializable]
    public class SoBindingSourceEntry
    {
        public int id;
        public object propertyObject;
        public Type objectType;
        public Action bind;
        public Action resetDelegates;
        public Func<Type> getValueType;
        public Func<object> getValue;
        public List<string> properties = new();
        public int selectedPropertyIndex;
        public List<string> subProperties = new();
        public int selectedSubIndex;


        public void Reflect(SerializedProperty prop)
        {
            properties.Clear();
            subProperties.Clear();

            prop.FindPropertyRelative("selectedProperty").stringValue = null;
            prop.serializedObject.ApplyModifiedProperties();
            prop.serializedObject.Update();

            resetDelegates?.Invoke();
            

            var obj = prop.FindPropertyRelative("obj").boxedValue;
            if (obj is null) return;

            PopulateFirstProp(obj);

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

        private void PopulateFirstProp(object selectedComponent)
        {
            var propertyInfos = selectedComponent.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var propertyInfo in propertyInfos) {
                if (propertyInfo.PropertyType == getValueType.Invoke() ||
                    propertyInfo.PropertyType.GetProperties().Any(info => info.PropertyType == getValueType.Invoke())) {
                    properties.Add(propertyInfo.Name);
                }
            }

            var fieldInfos = selectedComponent.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var fieldInfo in fieldInfos) {
                if (fieldInfo.FieldType == getValueType.Invoke() ||
                    fieldInfo.FieldType.GetProperties().Any(info => info.PropertyType == getValueType.Invoke())) {
                    properties.Add(fieldInfo.Name);
                }
            }
        }


        public void SubReflect(SerializedProperty prop)
        {
            subProperties.Clear();
            prop.FindPropertyRelative("selectedSub").stringValue = null;
            prop.serializedObject.ApplyModifiedProperties();
            prop.serializedObject.Update();

            resetDelegates.Invoke();

            var selectedProp = prop.FindPropertyRelative("selectedProperty").stringValue;
            if (selectedProp is null) return;
            var obj = prop.FindPropertyRelative("obj").boxedValue;
            PopulateSecondProp(obj, selectedProp);

            if (subProperties.Count > 0 && selectedSubIndex >= subProperties.Count) {
                selectedPropertyIndex = 0;
                prop.FindPropertyRelative("selectedSub").stringValue = subProperties[0];
                prop.serializedObject.ApplyModifiedProperties();
                prop.serializedObject.Update();
            }

            if (subProperties.Count > selectedSubIndex) {
                prop.FindPropertyRelative("selectedSub").stringValue = subProperties[selectedSubIndex];
                prop.serializedObject.ApplyModifiedProperties();
                prop.serializedObject.Update();
            }
        }

        private void PopulateSecondProp(object selectedComponent, string selectedProp)
        {
            var propertyInfo = selectedComponent.GetType().GetProperty(selectedProp);
            var subs = new PropertyInfo[] {
            };
            if (propertyInfo is not null) subs = propertyInfo.PropertyType.GetProperties();

            var fieldInfo = selectedComponent.GetType().GetField(selectedProp);
            if (fieldInfo is not null) subs = fieldInfo.FieldType.GetProperties();

            foreach (var sub in subs) {
                if (sub.PropertyType == getValueType.Invoke()) {
                    subProperties.Add(sub.Name);
                }
            }
        }


        public bool ReEvaluate(SerializedProperty prop)
        {
            ScriptableObject selectedComp;
            try {
                //try to check if object is null or missing ref
                selectedComp =
                    ((ScriptableObject)prop.FindPropertyRelative("obj").boxedValue);
            } catch {
                WipeAll(prop);
                return false;
            }
            //Now check property modification
            if (selectedComp is null) return false;
            PopulateFirstProp(selectedComp);

            var selectedProperty = prop.FindPropertyRelative("selectedProperty").stringValue;
            if (properties.Contains(selectedProperty)) selectedPropertyIndex = properties.IndexOf(selectedProperty);
            if (properties.Count > 0 && selectedPropertyIndex >= properties.Count) {
                selectedPropertyIndex = 0;
            }

            UpdateSelected(prop);

            //Check sub
            if (selectedProperty is null) return false;
            PopulateSecondProp(selectedComp, selectedProperty);

            var selectedSub = prop.FindPropertyRelative("selectedSub").stringValue;
            if (subProperties.Contains(selectedSub)) selectedSubIndex = subProperties.IndexOf(selectedSub);
            if (subProperties.Count > 0 && selectedSubIndex >= subProperties.Count) {
                selectedSubIndex = 0;
            }

            UpdateSelected(prop);

            bind.Invoke();
            return true;
        }

        public void UpdateSelected(SerializedProperty prop)
        {
            if (properties.Count > selectedPropertyIndex)
                prop.FindPropertyRelative("selectedProperty").stringValue = properties[selectedPropertyIndex];
            if (subProperties.Count > selectedSubIndex)
                prop.FindPropertyRelative("selectedSub").stringValue = subProperties[selectedSubIndex];
            prop.serializedObject.ApplyModifiedProperties();
            prop.serializedObject.Update();
        }

        private void WipeAll(SerializedProperty prop)
        {
            prop.FindPropertyRelative("obj").boxedValue = null;

            properties.Clear();
            prop.FindPropertyRelative("selectedProperty").stringValue = null;

            prop.FindPropertyRelative("selectedSub").stringValue = null;

            selectedSubIndex = 0;
            subProperties.Clear();
            resetDelegates.Invoke();
            prop.serializedObject.ApplyModifiedProperties();
            prop.serializedObject.Update();
        }
    }
}