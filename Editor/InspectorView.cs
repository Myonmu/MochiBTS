using MyonBTS.Core.Primitives;
using UnityEngine;
using UnityEngine.UIElements;
namespace MyonBTS.Editor
{
    public class InspectorView : VisualElement
    {
        private UnityEditor.Editor editor;
        public void UpdateSelection(NodeView nodeView)
        {
            Clear();
            Object.DestroyImmediate(editor);
            editor = UnityEditor.Editor.CreateEditor(nodeView.node);
            var container = new IMGUIContainer(() => {
                if (editor.target)
                    editor.OnInspectorGUI();
            });
            Add(container);
        }

        public void UpdateBlackBoard(BehaviorTree tree)
        {
            Clear();
            Object.DestroyImmediate(editor);
            if (tree.blackboard is null) return;
            editor = UnityEditor.Editor.CreateEditor(tree.blackboard);
            var container = new IMGUIContainer(() => {
                if (editor.target)
                    editor.OnInspectorGUI();
            });
            Add(container);
        }
        public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits>
        {
        }
    }
}