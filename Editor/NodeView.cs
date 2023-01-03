using System;
using MochiBTS.Core.Primitives.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Node = UnityEditor.Experimental.GraphView.Node;
namespace MochiBTS.Editor
{
    public sealed class NodeView : Node
    {
        public readonly Core.Primitives.Nodes.Node node;
        public Port input;
        public Action<NodeView> onNodeSelected;
        public Port output;
        public NodeView(Core.Primitives.Nodes.Node nodeRef) :
            base(AssetDatabase.GetAssetPath(BehaviorTreeSettings.GetOrCreateSettings().nodeXml))
        {
            var settings = BehaviorTreeSettings.GetOrCreateSettings();
            styleSheets.Add(settings.nodeStyle);
            // Construct the node view from fed Node instance
            node = nodeRef;
            title = nodeRef.name.Replace("Node", "");
            viewDataKey = node.guid;
            style.left = node.position.x;
            style.top = node.position.y;
            tooltip = node.Tooltip;


            //Construct ports
            CreateInputPorts();
            CreateOutputPorts();

            //Add base uss class labels
            SetupClasses();

            var serializedObject = new SerializedObject(node);
            //Set up description label
            var descriptionLabel = this.Q<Label>("description");
            descriptionLabel.bindingPath = "description";
            descriptionLabel.Bind(serializedObject);
            
            //Set up info label
            var infoLabel = this.Q<Label>("info");
            //infoLabel.RegisterCallback<>;
            infoLabel.bindingPath = "info";
            infoLabel.Bind(serializedObject);
            
            var subInfoLabel = this.Q<Label>("subInfo");
            subInfoLabel.bindingPath = "subInfo";
            subInfoLabel.Bind(serializedObject);

            var icon = this.Q<Image>("icon");
            
        }
        /// <summary>
        ///     Add the node's base class label. Allows to apply uss styling.
        /// </summary>
        private void SetupClasses()
        {
            switch (node) {
                case ActionNode:
                    AddToClassList("action");
                    break;
                case CompositeNode:
                    AddToClassList("composite");
                    break;
                case DecoratorNode:
                    AddToClassList("decorator");
                    break;
                case RootNode:
                    AddToClassList("root");
                    break;
            }
        }
        private void CreateOutputPorts()
        {
            switch (node) {
                case ActionNode:
                    break;
                case CompositeNode:
                    output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
                    break;
                case DecoratorNode:
                    output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
                    break;
                case RootNode:
                    output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
                    break;
            }
            if (output is null) return;
            output.portName = "";
            output.style.flexDirection = FlexDirection.ColumnReverse;
            outputContainer.Add(output);
        }
        private void CreateInputPorts()
        {
            switch (node) {
                case ActionNode:
                    input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
                    break;
                case CompositeNode:
                    input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
                    break;
                case DecoratorNode:
                    input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
                    break;
                case RootNode:
                    break;
            }
            if (input is null) return;
            input.portName = "";
            input.style.flexDirection = FlexDirection.Column;
            inputContainer.Add(input);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Undo.RecordObject(node, "Behaviour Tree (Set Position)");
            node.position.x = newPos.xMin;
            node.position.y = newPos.yMin;
            EditorUtility.SetDirty(node);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            onNodeSelected?.Invoke(this);
        }

        public void SortChildren()
        {
            var composite = node as CompositeNode;
            if (composite && composite.children is not null)
                composite.children.Sort(SortByHorizontalPosition);
        }
        private static int SortByHorizontalPosition(Core.Primitives.Nodes.Node x, Core.Primitives.Nodes.Node y)
        {
            return x.position.x < y.position.x ? -1 : 1;
        }

        public void UpdateState()
        {
            // Purge preexisting class labels
            RemoveFromClassList("running");
            RemoveFromClassList("failure");
            RemoveFromClassList("success");
            if (!Application.isPlaying) return;
            // Add uss class labels according to the node's state 
            switch (node.state) {
                case Core.Primitives.Nodes.Node.State.Running:
                    if (node.started) AddToClassList("running");
                    break;
                case Core.Primitives.Nodes.Node.State.Failure:
                    AddToClassList("failure");
                    break;
                case Core.Primitives.Nodes.Node.State.Success:
                    AddToClassList("success");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}