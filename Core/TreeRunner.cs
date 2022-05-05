using MochiBTS.Core.Primitives;
using MochiBTS.Core.Primitives.DataContainers;
using UnityEngine;
namespace MochiBTS.Core
{
    public class TreeRunner : MonoBehaviour
    {
        public BehaviorTree tree;
        //public bool useOriginal = false;
        public Agent agent;
        private void Start()
        {
            tree = tree.Clone();
            agent ??= GetComponent<Agent>();
        }

        private void Update()
        {
            tree.UpdateTree(agent);
        }

        public void ResetTree()
        {
            tree.ResetTree();
        }
    }
}