using System.Collections.Generic;
using DefaultNamespace.MochiVariable;
using UnityEngine;
namespace DefaultNamespace.TestGround
{
    public class TestDrawer : MonoBehaviour
    {
        public List<MochiVariable<float>> composites;

        private void Start()
        {
            foreach (var mochiVariable in composites) {
                mochiVariable.InitializeBinding();
            }
        }
    }
}