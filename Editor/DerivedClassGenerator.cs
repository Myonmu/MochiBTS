using System;
using System.Collections.Generic;
using MochiBTS.Core.Primitives.MochiVariable;
using UnityEditor;
using UnityEngine;
using File = System.IO.File;

namespace MochiBTS.Editor
{
    public class DerivedClassGenerator : EditorWindow
    {
        [SerializeField] private string templatePath;
        [MenuItem("MochiBTS/Derived Class Generator")]
        private static void ShowWindow()
        {
            var window = GetWindow<DerivedClassGenerator>();
            window.titleContent = new GUIContent("Derived Class Generator");
            window.Show();
        }

        private void OnGUI()
        {
            
        }

        private void Generate()
        {
            var template = File.ReadAllText(templatePath);
            
            var types = new List<Type>();
            foreach (var t in TypeCache.GetTypesDerivedFrom<IMochiVariableBase>())
            {
                if (t.IsGenericType && t.IsGenericTypeDefinition)
                {
                    var genericParam = t.GetGenericArguments()[0];
                    var derived = template.Replace("<@>",$"<{genericParam.Name}>");
                    derived = derived.Replace(" @ ", $"< {genericParam.Name} >");
                    
                }
            }
            
            
        }
    }
}