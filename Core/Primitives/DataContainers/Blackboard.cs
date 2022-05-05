﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
namespace MochiBTS.Core.Primitives.DataContainers
{
    [Serializable]
    public struct EventEntry
    {
        public string eventName;
        public ScriptableObject soEvent;
        public EventEntry(string name, ScriptableObject evt)
        {
            eventName = name;
            soEvent = evt;
        }
        public EventEntry Clone()
        {
            return new EventEntry(eventName, Object.Instantiate(soEvent));
        }
    }
    [Serializable]
    public class Blackboard : ScriptableObject
    {
        public List<EventEntry> btsEventEntries;


        public Blackboard Clone()
        {
            var instance = Instantiate(this);
            var eventInstances = btsEventEntries.Select(evt => evt.Clone()).ToList();
            instance.btsEventEntries = eventInstances;
            return instance;
        }
    }
}