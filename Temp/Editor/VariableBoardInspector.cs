using System.Collections.Generic;
using System.Linq;
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
                true, true, true, true) {
                //list.elementHeight = EditorGUIUtility.singleLineHeight * 2.5f;
                drawHeaderCallback = rect => EditorGUI.LabelField(rect, "Variables")
            };

            list.drawElementCallback =
                (rect, index, isActive, isFocused) => {
                    var element = list.serializedProperty.GetArrayElementAtIndex(index);
                    var namingConflict = !needRevalidateNames && namingConflictStats[index];
                    rect.x += 10f;
                    rect.width -= 10f;
                    rect.height = EditorGUIUtility.singleLineHeight;
                    if (needRevalidateNames) {
                        var s = element.FindPropertyRelative("key").stringValue;
                        if (varNames.Any(n => n == s)) {
                            namingConflict = true;
                        }
                        if(!namingConflict)varNames.Add(s);
                        namingConflictStats.Add(index,namingConflict);
                    }
                    var val = element.FindPropertyRelative("val");
                    var useBinding = element.FindPropertyRelative("bindVariable").boolValue;
                    var bindingSource = element.FindPropertyRelative("bindingSource");
                    CompactBindingSourceDrawer.InitWhenHidden(bindingSource);
                    var objOrig = ((GameObject)bindingSource.FindPropertyRelative("obj").boxedValue);
                    var obj = objOrig is null ? "null" : objOrig.name;
                    var compOrig = ((Component)bindingSource.FindPropertyRelative("selectedComponent").boxedValue);
                    var comp = compOrig is null ? "null" : compOrig.GetType().Name;
                    var prop = bindingSource.FindPropertyRelative("selectedProperty").stringValue;
                    
                    var valString = useBinding ?$"{obj}.{comp}.{prop} => {CompactBindingSourceDrawer.FetchValue(bindingSource)}" :$"{val.boxedValue}";
                    if (namingConflict) {
                        GUI.contentColor = Color.red;
                    }
                    else if (useBinding) GUI.contentColor = string.IsNullOrEmpty(prop)?Color.red:Color.cyan;
                    else GUI.contentColor = Color.yellow;
                    element.isExpanded = EditorGUI.Foldout(rect, element.isExpanded, namingConflict?$"[{index}] !!! NAMING CONFLICT !!!":
                        $"[{index}] {element.FindPropertyRelative("key").stringValue} = <{val.type}> {valString}", true);
                    GUI.contentColor = Color.white;
                    if (element.isExpanded) {

                        rect.x += 10f;
                        rect.y += EditorGUIUtility.singleLineHeight;
                        rect.width -= 10f;
                        rect.height -= EditorGUIUtility.singleLineHeight;
                        EditorGUI.PropertyField(rect, element, true);
                    }
                    if (needRevalidateNames && index == (list.count - 1)) {
                        needRevalidateNames = false;
                    }
                };


            list.elementHeightCallback = (index) => {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                var h = EditorGUIUtility.singleLineHeight;
                if (element.isExpanded)
                    h += EditorGUI.GetPropertyHeight(element);
                return h;
            };


            list.onChangedCallback += (l) => {
                //Debug.Log("ChangeDetected");
                CompactBindingSourceDrawer.InvalidateCache(list.serializedProperty);
                varNames.Clear();
                namingConflictStats.Clear();
                needRevalidateNames = true;
                //Repaint();
            };

            list.onSelectCallback += l => {
                foreach (var index in l.selectedIndices) {
                    var element = list.serializedProperty.GetArrayElementAtIndex(index);
                    if (!element.FindPropertyRelative("bindVariable").boolValue) continue;
                    var bindingSource = element.FindPropertyRelative("bindingSource");
                    CompactBindingSourceDrawer.ReEvaluateBinding(bindingSource);
                    varNames.Clear();
                    namingConflictStats.Clear();
                    needRevalidateNames = true;
                }
                
            };
            

        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
        
    }
}