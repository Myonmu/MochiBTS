using System.Collections.Generic;
using UnityEngine;
namespace MochiBTS.Core.Primitives.Utilities.Event
{
    [CreateAssetMenu(fileName = "BTSEvent", menuName = "BTS/BTS Event")]
    public class BtsEvent : ScriptableObject, IInvokable
    {
        [TextArea] [SerializeField] private string eventDescription;
        private readonly List<IListener> subscribers = new();
        
        public void Invoke()
        {
            foreach (var t in subscribers)
                t.OnEventReceive();
        }

        public static BtsEvent operator +(BtsEvent evt, IListener sub)
        {
            evt.subscribers.Add(sub);
            return evt;
        }

        public static BtsEvent operator -(BtsEvent evt, IListener sub)
        {
            evt.subscribers.Remove(sub);
            return evt;
        }
    }
}