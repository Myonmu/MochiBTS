using System;
using MochiBTS.Core.Primitives.Nodes;
using UnityEditor;

namespace MochiBTS.Editor
{
    [CustomEditor(typeof(Node),true)]
    public class NodeInspector : UnityEditor.Editor
    {

        private Node node;

        private void OnEnable()
        {
            node = target as Node;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            node.UpdateInfo();
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            serializedObject.Update();
        }
        
    }
}