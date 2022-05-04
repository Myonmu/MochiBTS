
using UnityEngine;
namespace MochiBTS.Core.Primitives.Utilities
{
    public class MovementTarget : MonoBehaviour
    {
        public static MovementTarget instance { get; private set; }
        private void Awake()
        {
            instance ??= this;
            if(instance!=this) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
        
    }
}