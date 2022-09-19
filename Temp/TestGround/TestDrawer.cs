using System;
using System.Collections.Generic;
using DefaultNamespace.MochiVariable;
using UnityEngine;
namespace DefaultNamespace.TestGround
{
    [Serializable]
    public struct TestComposite
    {
        public string key;
        public Vector3 val;
        public BindingSource<Vector3> bindingSource;
    }
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