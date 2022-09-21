using System.Collections.Generic;
using DefaultNamespace.MochiVariable;
using UnityEngine;
namespace DefaultNamespace.TestGround
{
    public class TestDrawer : MonoBehaviour
    {
        [SerializeReference]
        public List<IMochiVariableBase> composites = new ();

        private void Start()
        {
            foreach (var mochiVariable in composites) {
                mochiVariable.InitializeBinding();
            }
        }
    }
}