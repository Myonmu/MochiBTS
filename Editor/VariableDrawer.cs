using MyonBTS.Core.Primitives.Variables;
using UnityEditor;
using UnityEngine;
namespace MyonBTS.Editor
{
    [CustomPropertyDrawer(typeof(VariableFactory))]
    public class VariableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property is null) return;
            float[] widthes = { position.width * 0.2f, position.width * 0.5f, position.width * 0.3f };
            var varFactory = (VariableFactory)property.boxedValue;
            var i = 0;
            position.width = widthes[0];
            EditorGUI.PropertyField(position,property.FindPropertyRelative("type"), GUIContent.none);
            var typeOfVar = varFactory.GetVariableType(varFactory.type);
            var specificVar = property.FindPropertyRelative(typeOfVar.Name).Copy();
            var parentPath = specificVar.propertyPath;
            while (specificVar.NextVisible(true) && specificVar.propertyPath.StartsWith(parentPath) && i<2) {
                position.x += position.width;
                position.width = widthes[i++];
                EditorGUI.PropertyField(position, specificVar,  GUIContent.none);
            }
        }
       
    }
}