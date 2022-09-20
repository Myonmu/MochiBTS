using UnityEngine;

namespace DefaultNamespace.MochiVariable
{
    public abstract class BindingSource
    {
        public virtual Object unityObj { get; set; }
        public string selectedProperty;
        public string selectedSub;
        
    }
}