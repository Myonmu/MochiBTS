using System;
using System.Collections.Generic;
using UnityEngine;
namespace MyonBTS.Core.Primitives.Variables
{
    public class VariableBoard: MonoBehaviour
    {
        public List<VariableFactory> variableList;
        private Dictionary<string, BaseVariable> variables = new();
        
        [ContextMenu("test")]
        private void Show()
        {
            foreach (var v in variableList) {
                var produced = v.CreateVariable();
                variables.Add(produced.key,produced);
            }

            foreach (var (key,value) in variables) {
                Debug.Log($"{key} : {value.boxedValue}");
            }
        }
    }
}