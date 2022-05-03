using System;
using UnityEngine;
namespace MyonBTS.Core.Primitives.Variables
{
    public abstract class BaseVariable
    {
        public string key;
        [HideInInspector] public abstract Type type { get;}
        [HideInInspector]public abstract object boxedValue { get; set; }
    }
}