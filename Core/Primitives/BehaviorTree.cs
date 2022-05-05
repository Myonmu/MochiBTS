using System;
using System.Collections.Generic;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Nodes;
using UnityEditor;
using UnityEngine;
namespace MochiBTS.Core.Primitives
{
    [CreateAssetMenu(fileName = "BehaviorTree", menuName = "BTS/BehaviorTree")]
    public class BehaviorTree : ScriptableObject
    {
        // private BehaviorTree instance;
        // public BehaviorTree runtimeInstance => instance ? instance : (instance = this.Clone());
        public Node rootNode;
        public Node.State treeState = Node.State.Running;
        public List<Node> nodes = new();
        public Blackboard blackboard;
        //View Transform
        [HideInInspector] public Vector3 transformScale;
        [HideInInspector] public Vector3 transformPosition;
        public Node.State UpdateTree(Agent agent, bool forceExecute = false)
        {
            if (forceExecute || rootNode.state == Node.State.Running)
                treeState = rootNode.UpdateNode(agent, blackboard);
            return treeState;
        }

        private static void Traverse(Node node, Action<Node> visitor)
        {
            if (node is null) return;
            visitor.Invoke(node);
            var children = GetChildren(node);
            children.ForEach(n => Traverse(n, visitor));
        }

        public BehaviorTree Clone()
        {

            var tree = Instantiate(this);
            tree.rootNode = tree.rootNode.Clone();
            tree.nodes = new List<Node>();
            Traverse(tree.rootNode, n => {
                if (n is null) return;
                tree.nodes.Add(n);
            });
            tree.blackboard = blackboard.Clone();
            return tree;
        }

        public void ResetTree()
        {
            foreach (var node in nodes)
                node.ResetNode();
        }

        // public void Bind(Agent agent)
        // {
        //     Traverse(rootNode, node => {
        //         node.Bind(blackboard,agent);
        //     });
        // }

        #if UNITY_EDITOR
        public Node CreateNode(Type type)
        {
            var node = CreateInstance(type) as Node;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();

            Undo.RecordObject(this, "Behavior Tree (CreateNode)");
            nodes.Add(node);

            if (!Application.isPlaying)
                AssetDatabase.AddObjectToAsset(node, this);
            Undo.RegisterCreatedObjectUndo(node, "Behavior Tree (CreateNode)");
            AssetDatabase.SaveAssets();
            return node;
        }

        public void DeleteNode(Node node)
        {
            if (node is null) return;
            Undo.RecordObject(this, "Behavior Tree (DeleteNode)");
            nodes.Remove(node);
            //AssetDatabase.RemoveObjectFromAsset(node);
            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets();
        }

        public static void AddChild(Node parent, Node child)
        {
            if (parent is null || child is null) return;
            Undo.RecordObject(parent, "Behavior Tree (AddChild)");
            switch (parent) {
                case DecoratorNode decoratorNode:
                    decoratorNode.child = child;
                    break;
                case CompositeNode compositeNode:
                    compositeNode.children.Add(child);
                    break;
                case RootNode root:
                    root.child = child;
                    break;
            }
            EditorUtility.SetDirty(parent);

        }

        public static void RemoveChild(Node parent, Node child)
        {
            if (parent is null || child is null) return;
            Undo.RecordObject(parent, "Behavior Tree (RemoveChild)");
            switch (parent) {
                case DecoratorNode decoratorNode:
                    decoratorNode.child = null;
                    break;
                case CompositeNode compositeNode:
                    compositeNode.children.Remove(child);
                    break;
                case RootNode root:
                    root.child = null;
                    break;
            }
            EditorUtility.SetDirty(parent);

        }

        public static List<Node> GetChildren(Node parent)
        {
            var children = new List<Node>();
            switch (parent) {
                case DecoratorNode { child: {} } decoratorNode:
                    children.Add(decoratorNode.child);
                    break;
                case CompositeNode compositeNode:
                    return compositeNode.children;
                case RootNode { child: {} } root:
                    children.Add(root.child);
                    break;
            }

            return children;
        }

        #endif
    }
}