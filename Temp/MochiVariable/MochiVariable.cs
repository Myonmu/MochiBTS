using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
namespace DefaultNamespace.MochiVariable
{
    [Serializable]
    public class MochiVariable<T>: IMochiVariableBase
    {
        public string key;
        [SerializeField] private bool bindVariable = false;
        [SerializeField] private T val;
        [SerializeField] private SoBindingSource<T> bindingSource;

        public void InitializeBinding()
        {
            //Delegates are not serialized, therefore each mochi var must be rebound at the start of the game.
            bindingSource.Bind();
        }
        
        public T value {
            get => bindingSource?.getValue != null ? bindingSource.getValue.Invoke() : val;
            set {
                if (bindingSource?.setValue != null) {
                    bindingSource.setValue.Invoke(value);
                    return;
                }
                val = value;
            }
        }

    }
}