using System;
using UnityEngine;
namespace DefaultNamespace.MochiVariable
{
    public enum BindingMode
    {
        Value,
        GO,
        SO
    }
    [Serializable]
    public class MochiVariable<T>: IMochiVariableBase
    {
        public string key;
        [SerializeField] private BindingMode bindVariable;
        [SerializeField] private T val;
        [SerializeField] private GoBindingSource<T> goBindingSource;
        [SerializeField] private SoBindingSource<T> soBindingSource;

        public void InitializeBinding()
        {
            //Delegates are not serialized, therefore each mochi var must be rebound at the start of the game.
            switch (bindVariable) {

                case BindingMode.Value:
                    break;
                case BindingMode.GO:
                    goBindingSource.Bind();
                    break;
                case BindingMode.SO:
                    soBindingSource.Bind();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public T value {
            get {
                return bindVariable switch {
                    BindingMode.Value => val,
                    BindingMode.GO => goBindingSource.getValue.Invoke(),
                    BindingMode.SO => soBindingSource.getValue.Invoke(),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            set {
                switch (bindVariable) {
                    case BindingMode.Value:
                        val = value;
                        break;
                    case BindingMode.GO:
                        goBindingSource.setValue.Invoke(value);
                        break;
                    case BindingMode.SO:
                        soBindingSource.setValue.Invoke(value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

    }
}