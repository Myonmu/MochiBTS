using System;
using MochiBTS.Core;
using MochiBTS.Core.Primitives.Events;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
namespace MochiBTS.Editor
{
    [CustomEditor(typeof(TreeRunner))]
    public class TreeRunnerInspector : UnityEditor.Editor
    {
        private TreeRunner runner;
        private void OnEnable()
        {
            runner = target as TreeRunner;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (runner.executionMode is not ExecutionMode.Event) return;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Event");
            runner.trigger = EditorGUILayout.ObjectField(runner.trigger,typeof(ISubscribable),false) as ScriptableObject;
            EditorGUILayout.EndHorizontal();
        }
    }
}