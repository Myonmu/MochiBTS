using MochiBTS.Core.Primitives.Variables;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
namespace MochiBTS.Editor
{
    [CustomEditor(typeof(VariableBoard))]
    public class VariableBoardInspector : UnityEditor.Editor
    {
        private ReorderableList reorderableList;

        private void OnEnable()
        {
            reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("variableList"));
            reorderableList.drawElementCallback += (rect, index, active, focused) => {
                var property = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, property, GUIContent.none);
            };
            reorderableList.drawHeaderCallback += rect => {
                EditorGUI.LabelField(rect, "Type --- Key --- Value");
            };
        }

        public override void OnInspectorGUI()
        {
            reorderableList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}