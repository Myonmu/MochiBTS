using MochiBTS.Core.Primitives;
using MochiBTS.Core.Primitives.DataContainers;
using MochiBTS.Core.Primitives.Events;
using UnityEngine;
namespace MochiBTS.Core
{
    public enum ExecutionMode
    {
        Update,
        FixedUpdate,
        Event
    }
    public class TreeRunner : MonoBehaviour, IListener
    {
        public BehaviorTree tree;
        //public bool useOriginal = false;
        public Agent agent;
        public ExecutionMode executionMode = ExecutionMode.Update;
        [HideInInspector] public ScriptableObject trigger; //Only showed on demand
        private void Start()
        {
            tree = tree.Clone();
            ResetTree();
            agent ??= GetComponent<Agent>();
            
        }

        private void OnEnable()
        {
            if (executionMode is ExecutionMode.Event && trigger is ISubscribable subscribable) {
                subscribable.Subscribe(this);
            }
        }

        private void OnDisable()
        {
            if (executionMode is ExecutionMode.Event && trigger is ISubscribable subscribable) {
                subscribable.Unsubscribe(this);
            }
        }

        private void Update()
        {
            if (executionMode is not ExecutionMode.Update) return;
            OnEventReceive();
        }

        private void FixedUpdate()
        {
            if (executionMode is not ExecutionMode.FixedUpdate) return;
           OnEventReceive();
        }

        public void ResetTree()
        {
            tree.ResetTree();
        }
        public void OnEventReceive()
        {
            tree.UpdateTree(agent);
        }
    }
}