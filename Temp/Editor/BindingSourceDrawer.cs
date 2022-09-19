using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using DefaultNamespace.MochiVariable;
using UnityEditor;
using UnityEngine;
namespace DefaultNamespace.Editor
{
    [CustomPropertyDrawer(typeof(BindingSource<>))]
    public class BindingSourceDrawer : PropertyDrawer
    {
        /* Since all binding source share the same property drawer, we keep a registry of all the instances
         * to avoid using reflections for initialization. (trade memory for performance)
         */
        public struct BindingSourceEntry
        {
             public int id;
             public object propertyObject;
             public Type objectType;
             public Action refresh;
             public Action reflect;
             public Action bind;
             public Action reEval;
             public Func<object> getValue;
        }
        private static readonly Regex MatchArrayElement = new(@"data\[(\d+)\]$");
        //private static readonly Dictionary<string, BindingSourceEntry> Entries = new();
        private static readonly Dictionary<int, Dictionary<string, BindingSourceEntry>> Entries = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var entry = Initialize(property);
            var maxWidth = position.width;
            EditorGUI.BeginProperty(position, label, property);
            position.width = 0.3f * maxWidth;
            EditorGUI.BeginChangeCheck();
            property.serializedObject.Update();
            EditorGUI.PropertyField(position, property.FindPropertyRelative("obj"), GUIContent.none);
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
            if (EditorGUI.EndChangeCheck()) {
                //Debug.Log($"{property.propertyPath},idx{index},init{initialized}");
                //property.serializedObject.Update();
                entry.refresh?.Invoke();
                entry.reflect?.Invoke();
                property.serializedObject.ApplyModifiedProperties();
                property.serializedObject.Update();
            }
            position.width = maxWidth * 0.35f;
            position.x += 0.3f * maxWidth;
            var selectedID = property.FindPropertyRelative("selectedComponentIndex");

            if (entry.propertyObject.GetType().GetField("componentLabels").GetValue(entry.propertyObject) is List<string> list) {
                //Debug.Log("Evaluating popup");
                EditorGUI.BeginChangeCheck();
                property.serializedObject.Update();
                selectedID.intValue = EditorGUI.Popup(position, selectedID.intValue, list.ToArray());
                property.serializedObject.ApplyModifiedProperties();
                if (EditorGUI.EndChangeCheck()) {
                    //property.serializedObject.Update();
                    entry.reflect?.Invoke();
                    entry.bind?.Invoke();
                    property.serializedObject.ApplyModifiedProperties();
                    property.serializedObject.Update();
                    
                }
            }

            position.x += 0.35f * maxWidth;
            var selectedProp = property.FindPropertyRelative("selectedPropertyIndex");
            if (entry.propertyObject.GetType().GetField("properties").GetValue(entry.propertyObject) is List<string> props) {
                if (props.Count < 1) {
                    EditorGUI.HelpBox(position,"Unavailable",MessageType.Warning);
                } else {
                    EditorGUI.BeginChangeCheck();
                    property.serializedObject.Update();
                    selectedProp.intValue = EditorGUI.Popup(position, selectedProp.intValue, props.ToArray());
                    property.serializedObject.ApplyModifiedProperties();
                    if (EditorGUI.EndChangeCheck()) {
                        //property.serializedObject.Update();
                        entry.bind?.Invoke();
                        property.serializedObject.ApplyModifiedProperties();
                        property.serializedObject.Update();
                    }
                }

            }
            EditorGUI.EndProperty();
        }
        

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public static void ReEvaluateBinding(SerializedProperty prop)
        {
            var path = prop.propertyPath;
            var id = prop.serializedObject.targetObject.GetInstanceID();
            if (Entries.ContainsKey(id)&& Entries[id].ContainsKey(path)) { 
                var target = Entries[id][path];
                target.reEval?.Invoke();
            }
        }


        protected static BindingSourceEntry Initialize(SerializedProperty prop)
        {
            var serializedObject = prop.serializedObject; 
            var path = prop.propertyPath;
            var id = prop.serializedObject.targetObject.GetInstanceID();
            /*
            var firstMatch = MatchArrayElement.Match(path);
            if (firstMatch.Success && int.TryParse(firstMatch.Groups[1].Value, out var id)) {
                if (Entries.ContainsKey(key)) {
                    var target = Entries[key];
                    if (target.id != id) {
                        Entries.Clear();
                    }
                }
            }
            */

            if (Entries.ContainsKey(id)&& Entries[id].ContainsKey(path)) { 
                var target = Entries[id][path];
                return target;
            }
            
            BindingSourceEntry entry = new();
            //if (initialized) return;
            //Debug.Log($"Initialize {id+path}");
            entry.propertyObject = serializedObject == null || serializedObject.targetObject == null ? null : serializedObject.targetObject;
            entry.objectType = entry.propertyObject?.GetType();
            if (!string.IsNullOrEmpty(path) && entry.propertyObject != null) {
                var splitPath = path.Split('.');
                Type fieldType = null;

                //work through the given property path, node by node
                for (var i = 0; i < splitPath.Length; i++) {
                    var pathNode = splitPath[i];

                    //both arrays and lists implement the IList interface
                    if (fieldType != null && typeof(IList).IsAssignableFrom(fieldType)) {
                        //IList items are serialized like this: `Array.data[0]`
                        Debug.AssertFormat(pathNode.Equals("Array", StringComparison.Ordinal), serializedObject.targetObject, "Expected path node 'Array', but found '{0}'", pathNode);

                        //just skip the `Array` part of the path
                        pathNode = splitPath[++i];

                        //match the `data[0]` part of the path and extract the IList item index
                        var elementMatch = MatchArrayElement.Match(pathNode);
                        if (elementMatch.Success && int.TryParse(elementMatch.Groups[1].Value, out  entry.id)) {
                            var objectArray = (IList)entry.propertyObject;
                            var validArrayEntry = objectArray != null && entry.id < objectArray.Count;
                            entry.propertyObject = validArrayEntry ? objectArray[entry.id] : null;
                            entry.objectType = fieldType.IsArray
                                ? fieldType.GetElementType() //only set for arrays
                                : fieldType.GenericTypeArguments[0]; //type of `T` in List<T>
                        } else {
                            Debug.LogErrorFormat(serializedObject.targetObject, "Unexpected path format for array item: '{0}'", pathNode);
                        }
                        //reset fieldType, so we don't end up in the IList branch again next iteration
                        fieldType = null;
                    } else {
                        FieldInfo field;
                        var instanceType = entry.objectType;
                        var fieldBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
                        do {
                            field = instanceType.GetField(pathNode, fieldBindingFlags);

                            //b/c a private, serialized field of a subclass isn't directly retrievable,
                            fieldBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
                            //if necessary, work up the inheritance chain until we find it.
                            instanceType = instanceType.BaseType;
                        } while (field == null && instanceType != typeof(object));

                        //store object info for next iteration or to return
                        entry.propertyObject = field == null || entry.propertyObject == null ? null : field.GetValue(entry.propertyObject);
                        fieldType = field == null ? null : field.FieldType;
                        entry.objectType = fieldType;
                    }
                }
            }
            if (entry.propertyObject != null) {
                entry.refresh = (Action)entry.propertyObject.GetType().GetMethod("Refresh")?.CreateDelegate(typeof(Action), entry.propertyObject);
                entry.reflect = (Action)entry.propertyObject.GetType().GetMethod("Reflect")?.CreateDelegate(typeof(Action), entry.propertyObject);
                entry.bind = (Action)entry.propertyObject.GetType().GetMethod("Bind")?.CreateDelegate(typeof(Action), entry.propertyObject);
                entry.reEval = (Action)entry.propertyObject.GetType().GetMethod("ReEvaluate")?.CreateDelegate(typeof(Action), entry.propertyObject);
                entry.getValue = (Func<object>)entry.propertyObject.GetType().GetMethod("BoxedValue")?.CreateDelegate(typeof(Func<object>), entry.propertyObject);
            }
            //initialized = true;
            if (!Entries.ContainsKey(id)) {
                Entries.Add(id,new Dictionary<string, BindingSourceEntry>());
            }
            Entries[id].Add(path,entry);
            entry.bind?.Invoke();
            return entry;
        }

        public static void InvalidateCache()
        {
            Entries.Clear();
        }

        public static void InitWhenHidden(SerializedProperty prop)
        {
            Initialize(prop);
        }
        

        public static void InvalidateCache(SerializedProperty prop)
        {
            
            var id = prop.serializedObject.targetObject.GetInstanceID();
            if (!Entries.ContainsKey(id)) return;
            Entries[id].Clear();
        }

        public static string FetchValue(SerializedProperty prop)
        {
            //return "";
            var path = prop.propertyPath;
            var id = prop.serializedObject.targetObject.GetInstanceID();
            if (!Entries.ContainsKey(id) || !Entries[id].ContainsKey(path)) return "NULL";
            var target = Entries[id][path];
            if (target.getValue is null) return "Not Bound";
            var result = target.getValue.Invoke();
            return result is null? "NULL":result.ToString();
        }
    }
}