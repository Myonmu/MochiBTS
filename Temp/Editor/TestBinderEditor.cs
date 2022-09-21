using DefaultNamespace.TestGround;
using UnityEditor;
using UnityEngine;
namespace DefaultNamespace.Editor
{
    [CustomEditor(typeof(TestBinder))]
    public class TestBinderEditor : UnityEditor.Editor
    {
        private TestBinder binder;
        //[SerializeField] private int selectedComp = 0;
        //private int selectedComponentID = -1;
        //private string[] componentsCache;
        private void OnEnable()
        {
            //componentsCache = null;
            binder = target as TestBinder;
        }
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Refresh")) {
                binder.Refresh();
            }
            if (GUILayout.Button("Reflect")) {
                binder.Reflect();
            }
            if (GUILayout.Button("Bind")) {
                binder.Bind();
            }
            base.OnInspectorGUI();
            if (binder.componentLabels.ToArray().Length<1) return;
            EditorGUI.BeginChangeCheck();
            binder.selectedComponentIndex = EditorGUILayout.Popup(new GUIContent("Component"),
                binder.selectedComponentIndex, binder.componentLabels.ToArray());
            if (EditorGUI.EndChangeCheck()) {
                binder.selectedComponentIndex = Mathf.Clamp(binder.selectedComponentIndex, 0, binder.selectedComponentIndex);
                binder.selectedComponent = binder.components[binder.selectedComponentIndex];
                binder.Reflect();
            }
            if (binder.properties.Count < 1) {
                EditorGUILayout.HelpBox("No property of matching type can be found.",MessageType.Warning);
                return;
            }
            EditorGUI.BeginChangeCheck();
            binder.selectedPropertyIndex = EditorGUILayout.Popup(new GUIContent("Property"),
                binder.selectedPropertyIndex, binder.properties.ToArray());
            if (EditorGUI.EndChangeCheck()) {
                binder.selectedProperty = binder.properties[Mathf.Clamp(binder.selectedPropertyIndex, 0, binder.selectedPropertyIndex)];
                binder.Bind();
            }
            if (binder.getter is not null) {
                binder.value = binder.getter.Invoke();
            }
        }
    }
}