using System;
using MochiBTS.Core.Primitives.Nodes;
using UnityEditor;
using UnityEngine.UIElements;

namespace MochiBTS.Editor
{
    [Obsolete]
    //[CustomEditor(typeof(Node),true)]
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