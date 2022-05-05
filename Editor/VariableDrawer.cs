using MochiBTS.Core.Primitives.Variables;
using UnityEditor;
using UnityEngine;
namespace MochiBTS.Editor
{
    [CustomPropertyDrawer(typeof(VariableFactory))]
    public class VariableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property is null) return;
            var varFactory = (VariableFactory)property.boxedValue;
            var originalWidth = position.width;
            position.width = 0.2f * originalWidth;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("type"), GUIContent.none);
            var typeOfVar = varFactory.GetVariableType();
            var specificVar = property.FindPropertyRelative(typeOfVar.Name).Copy();
            position.x += position.width + originalWidth * 0.02f;
            EditorGUI.PropertyField(position, specificVar.FindPropertyRelative("key"), GUIContent.none);
            position.x += position.width + originalWidth * 0.02f;
            position.width = 0.5f * originalWidth;
            EditorGUI.PropertyField(position, specificVar.FindPropertyRelative("value"), GUIContent.none);
        }
    }
}