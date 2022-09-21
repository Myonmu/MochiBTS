using DefaultNamespace.MochiVariable;
using UnityEditor;
using UnityEngine;
namespace DefaultNamespace.Editor
{
    [CustomPropertyDrawer(typeof(MochiVariable<>))]
    public class MochiVariableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = true;
            position.x += 7f;
            var maxWidth = position.width;
            var initX = position.x;

            //Key
            position.height = EditorGUIUtility.singleLineHeight;
            position.width = maxWidth * 0.3f;
            EditorGUI.BeginProperty(position, label, property);
            property.serializedObject.Update();
            EditorGUI.PropertyField(position, property.FindPropertyRelative("key"), GUIContent.none);
            property.serializedObject.ApplyModifiedProperties();
            //Mode
            position.x += maxWidth * 0.3f + 7f;
            position.width = maxWidth * 0.1f;
            var mode = property.FindPropertyRelative("bindVariable");
            property.serializedObject.Update();
            EditorGUI.PropertyField(position, mode, GUIContent.none);
            property.serializedObject.ApplyModifiedProperties();
            //Value field
            position.x += maxWidth * 0.1f + 7f;
            position.width = maxWidth * 0.6f - 21f;
            var enumMode = mode.enumNames[mode.enumValueIndex];
            GUI.enabled = enumMode == "Value";
            var val = property.FindPropertyRelative("val");
            property.serializedObject.Update();

            EditorGUI.PropertyField(position, val, GUIContent.none);
            property.serializedObject.ApplyModifiedProperties();
            //Binding Source
            if (enumMode == "Value") {
                return;
            }

            if (!property.isExpanded) {
                return;
            }
            GUI.enabled = mode.boolValue;
            position.y += EditorGUIUtility.singleLineHeight + 7f;
            position.x = initX;
            position.width = maxWidth;
            switch (enumMode) {
                case "GO":
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("goBindingSource"), GUIContent.none);
                    break;
                case "SO":
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("soBindingSource"), GUIContent.none);
                    break;
            }

            GUI.enabled = true;
            EditorGUI.EndProperty();


        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.FindPropertyRelative("bindVariable").boolValue) return EditorGUIUtility.singleLineHeight;
            return EditorGUIUtility.singleLineHeight * (property.isExpanded ? 2.7f : 1);
        }
    }
}