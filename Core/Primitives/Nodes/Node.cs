using MochiBTS.Core.Primitives.DataContainers;
using UnityEngine;
namespace MochiBTS.Core.Primitives.Nodes
{
    public abstract class Node : ScriptableObject
    {
        public enum State
        {
            Running,
            Failure,
            Success
        }
        [HideInInspector] public State state = State.Running;
        [HideInInspector] public bool started;
        [HideInInspector] public string guid;
        [HideInInspector] public Vector2 position;
        //[HideInInspector] public Blackboard blackboard;
        //[HideInInspector] public Agent agent;
        [TextArea] public string description;
        public virtual string Tooltip { get; }

        public State UpdateNode(Agent agent, Blackboard blackboard)
        {
            if (!started) {
                OnStart(agent, blackboard);
                started = true;
            }
            state = OnUpdate(agent, blackboard);
            if (state == State.Running)
                return state;
            OnStop(agent, blackboard);
            started = false;
            return state;
        }

        public virtual Node Clone()
        {
            return Instantiate(this);
        }

        // public virtual void Bind(Blackboard b, Agent a)
        // {
        //     blackboard = b;
        //     agent = a;
        // }

        public virtual void ResetNode()
        {
            started = false;
            state = State.Running;
        }
        protected abstract void OnStart(Agent agent, Blackboard blackboard);
        protected abstract void OnStop(Agent agent, Blackboard blackboard);
        protected abstract State OnUpdate(Agent agent, Blackboard blackboard);
    }
}