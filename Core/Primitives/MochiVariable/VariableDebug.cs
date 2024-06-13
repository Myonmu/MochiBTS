using MochiBTS.Core.Primitives.DataContainers;
using Sirenix.OdinInspector;
using UnityEngine;
namespace MochiBTS.Core.Primitives.MochiVariable
{
    public class VariableDebug : MonoBehaviour
    {
        private VariableBoard variableBoard;
        public DataSource<GameObject> go;
        [Button]
        public void Debug1()
        {
            variableBoard = GetComponent<VariableBoard>();
            variableBoard.ConvertToDictionary();
            var x = variableBoard.GetValue<GameObject>("a");
            Debug.Log(x.name);
        }

        public void Debug2()
        {

        }
    }
}