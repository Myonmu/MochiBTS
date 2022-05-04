using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace MochiBTS.Core.Primitives.Variables
{
    [Serializable]
    public class VariableBoard: MonoBehaviour
    {
        public List<VariableFactory> variableList;
        private readonly Dictionary<string, BaseVariable> variables = new();

        private void Awake()
        {
            InitializeDict();
        }

        private void InitializeDict()
        {
            foreach (var produced in variableList.Select(v => v.CreateVariable())) {
                variables.Add(produced.key,produced);
            }
        }

        [ContextMenu("test")]
        private void Show()
        {
            InitializeDict();
            foreach (var (key,value) in variables) {
                Debug.Log($"{key} : {value.boxedValue}");
            }
        }

        public T GetValue<T>(string key)
        {
            return (T)variables[key].boxedValue;
        }

        public List<string> GetEntrySet()
        {
            return variables.Keys.ToList();
        }
    }
}