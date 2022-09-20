using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.MochiVariable;
using DefaultNamespace.TestGround;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DefaultNamespace.Editor
{
    [CustomEditor(typeof(TestDrawer))]
    public class VariableBoardInspector : UnityEditor.Editor
    {
        private readonly List<string> varNames = new();
        private bool needRevalidateNames = true;
        private Dictionary<int, bool> namingConflictStats = new();
        private ReorderableList list;

        private void OnEnable()
        {
            needRevalidateNames = true;
            var prop = serializedObject.FindProperty("composites");
            list = new ReorderableList(serializedObject, prop,
                true, true, true, true)
            {
                //list.elementHeight = EditorGUIUtility.singleLineHeight * 2.5f;
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Variables")
            };

            list.drawElementCallback =
                (rect, index, isActive, isFocused) =>
                {
                    var element = list.serializedProperty.GetArrayElementAtIndex(index);
                    var namingConflict = !needRevalidateNames && namingConflictStats[index];
                    rect.x += 10f;
                    rect.width -= 10f;
                    rect.height = EditorGUIUtility.singleLineHeight;
                    var key = element.FindPropertyRelative("key").stringValue;
                    if (needRevalidateNames)
                    {
                        if (varNames.Any(n => n == key))
                        {
                            namingConflict = true;
                        }

                        if (!namingConflict) varNames.Add(key);
                        namingConflictStats.Add(index, namingConflict);
                    }

                    var val = element.FindPropertyRelative("val");
                    var useBinding = element.FindPropertyRelative("bindVariable");
                    var mode = useBinding.enumNames[useBinding.enumValueIndex];
                    var valString = $"{val.boxedValue}";
                    var boundValueIsNull = false;
                    if (mode!="Value") {
                        valString = mode switch {
                            "GO" => ValStringFromGo(element, out boundValueIsNull),
                            "SO" => ValStringFromSo(element, out boundValueIsNull),
                            _ => valString
                        };
                    }

                    var voidName = string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key);
                    if (namingConflict || voidName)
                    {
                        GUI.contentColor = Color.red;
                    }
                    else if (mode!="Value") GUI.contentColor = boundValueIsNull ? Color.red : Color.cyan;
                    else GUI.contentColor = Color.yellow;

                    element.isExpanded = EditorGUI.Foldout(rect, element.isExpanded,
                        voidName
                            ? $"[{index}] !!! EMPTY VARIABLE NAME !!!"
                            : namingConflict
                                ? $"[{index}] !!! NAMING CONFLICT !!!"
                                : $"[{index}] {element.FindPropertyRelative("key").stringValue} = <{val.type}> {valString}",
                        true);

                    GUI.contentColor = Color.white;
                    if (element.isExpanded)
                    {
                        rect.x += 10f;
                        rect.y += EditorGUIUtility.singleLineHeight;
                        rect.width -= 10f;
                        rect.height -= EditorGUIUtility.singleLineHeight;
                        EditorGUI.PropertyField(rect, element, true);
                    }

                    if (needRevalidateNames && index == (list.count - 1))
                    {
                        needRevalidateNames = false;
                    }
                };


            list.elementHeightCallback = (index) =>
            {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                var h = EditorGUIUtility.singleLineHeight;
                if (element.isExpanded)
                    h += EditorGUI.GetPropertyHeight(element);
                return h;
            };


            list.onChangedCallback += (l) =>
            {
                //Debug.Log("ChangeDetected");
                SoBindingSourceDrawer.InvalidateCache(list.serializedProperty);
                GoBindingSourceDrawer.InvalidateCache(list.serializedProperty);
                varNames.Clear();
                namingConflictStats.Clear();
                needRevalidateNames = true;
                //Repaint();
            };

            list.onSelectCallback += l =>
            {
                foreach (var index in l.selectedIndices)
                {
                    var element = list.serializedProperty.GetArrayElementAtIndex(index);
                    if (!element.FindPropertyRelative("bindVariable").boolValue) continue;
                    var bindingSource = element.FindPropertyRelative("bindingSource");
                    SoBindingSourceDrawer.ReEvaluateBinding(bindingSource);
                    GoBindingSourceDrawer.ReEvaluateBinding(bindingSource);
                    varNames.Clear();
                    namingConflictStats.Clear();
                    needRevalidateNames = true;
                }
            };
        }
        private static string ValStringFromGo(SerializedProperty element, out bool boundValueIsNull)
        {

            var bindingSource = element.FindPropertyRelative("goBindingSource");
            GoBindingSourceDrawer.InitWhenHidden(bindingSource);
            var objOrig = (GameObject)bindingSource.FindPropertyRelative("obj").boxedValue;
            var obj = objOrig is null ? "null" : objOrig.name;
            var compOrig = ((Component)bindingSource.FindPropertyRelative("selectedComponent").boxedValue);
            var comp = compOrig is null ? "null" : compOrig.GetType().Name;
            var prop = bindingSource.FindPropertyRelative("selectedProperty").stringValue;
            var sub = bindingSource.FindPropertyRelative("selectedSub").stringValue;
            var valString = $"{obj}.{comp}.{prop + (string.IsNullOrEmpty(sub) ? "" : $".{sub}")} => {GoBindingSourceDrawer.FetchValue(bindingSource)}";
            boundValueIsNull = string.IsNullOrEmpty(prop) ||
                               (string.IsNullOrEmpty(prop) && string.IsNullOrEmpty(sub));
            return valString;
        }

        private static string ValStringFromSo(SerializedProperty element, out bool boundValueIsNull)
        {
            var bindingSource = element.FindPropertyRelative("soBindingSource");
            SoBindingSourceDrawer.InitWhenHidden(bindingSource);
            var objOrig = (ScriptableObject)bindingSource.FindPropertyRelative("obj").boxedValue;
            var obj = objOrig is null ? "null" : objOrig.name;
            var prop = bindingSource.FindPropertyRelative("selectedProperty").stringValue;
            var sub = bindingSource.FindPropertyRelative("selectedSub").stringValue;
            var valString = $"{obj}.{prop + (string.IsNullOrEmpty(sub) ? "" : $".{sub}")} => {SoBindingSourceDrawer.FetchValue(bindingSource)}";
            boundValueIsNull = string.IsNullOrEmpty(prop) ||
                               (string.IsNullOrEmpty(prop) && string.IsNullOrEmpty(sub));
            return valString;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        public static List<Type> FetchAllVariableTypes()
        {
            var cache = TypeCache.GetTypesDerivedFrom<IMochiVariableBase>();
            return cache.Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition).ToList();
        }

        public static IMochiVariableBase Instantiate(Type varType)
        {
            return (IMochiVariableBase)Activator.CreateInstance(varType);
        }
    }
}