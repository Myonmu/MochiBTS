using System.Collections.Generic;
using UnityEngine;
namespace MochiBTS.Core.Primitives.Events
{
    [CreateAssetMenu(fileName = "BTSEvent", menuName = "BTS/BTS Event")]
    public class BtsEvent : ScriptableObject, IListener, ISubscribable
    {
        [TextArea] [SerializeField] private string eventDescription;
        public bool isActive = true;
        private readonly List<IListener> subscribers = new();

        public void OnEventReceive()
        {
            if (!isActive) return;
            foreach (var t in subscribers)
                t.OnEventReceive();
        }
        public void Subscribe(IListener listener)
        {
            subscribers.Add(listener);
        }
        public void Unsubscribe(IListener listener)
        {
            subscribers.Remove(listener);
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