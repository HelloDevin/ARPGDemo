using System;
using System.Collections.Generic;

namespace ZZZ
{
    public enum EventName
    {
        TakeDamage,
        QTE
    }

    public class EventManager : Singleton<EventManager>
    {
        public delegate void EventDelegate();

        public delegate void EventDelegate<T1>(T1 t1);

        public delegate void EventDelegate<T1, T2>(T1 t1, T2 t2);

        public delegate void EventDelegate<T1, T2, T3>(T1 t1, T2 t2, T3 t3);

        public delegate void EventDelegate<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4);

        public delegate void EventDelegate<T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);

        private readonly Dictionary<EventName, Delegate> _eventListeners = new();

        public void RegisterEvent(EventName eventName, EventDelegate handler)
        {
            InternalRegisterEvent(eventName, handler);
        }

        public void RegisterEvent<T1>(EventName eventName, EventDelegate<T1> handler)
        {
            InternalRegisterEvent(eventName, handler);
        }

        public void RegisterEvent<T1, T2>(EventName eventName, EventDelegate<T1, T2> handler)
        {
            InternalRegisterEvent(eventName, handler);
        }

        public void RegisterEvent<T1, T2, T3>(EventName eventName, EventDelegate<T1, T2, T3> handler)
        {
            InternalRegisterEvent(eventName, handler);
        }

        public void RegisterEvent<T1, T2, T3, T4>(EventName eventName, EventDelegate<T1, T2, T3, T4> handler)
        {
            InternalRegisterEvent(eventName, handler);
        }

        public void RegisterEvent<T1, T2, T3, T4, T5>(EventName eventName, EventDelegate<T1, T2, T3, T4, T5> handler)
        {
            InternalRegisterEvent(eventName, handler);
        }

        private void InternalRegisterEvent(EventName eventName, Delegate handler)
        {
            if (!_eventListeners.TryAdd(eventName, handler))
            {
                _eventListeners[eventName] = Delegate.Combine(_eventListeners[eventName], handler);
            }
        }

        public void RemoveEvent(EventName eventName, EventDelegate handler)
        {
            if (_eventListeners.ContainsKey(eventName))
            {
                InternalRemoveEvent(eventName, handler);
            }
        }

        public void RemoveEvent<T1>(EventName eventName, EventDelegate<T1> handler)
        {
            if (_eventListeners.ContainsKey(eventName))
            {
                InternalRemoveEvent(eventName, handler);
            }
        }

        public void RemoveEvent<T1, T2>(EventName eventName, EventDelegate<T1, T2> handler)
        {
            if (_eventListeners.ContainsKey(eventName))
            {
                InternalRemoveEvent(eventName, handler);
            }
        }

        public void RemoveEvent<T1, T2, T3>(EventName eventName, EventDelegate<T1, T2, T3> handler)
        {
            if (_eventListeners.ContainsKey(eventName))
            {
                InternalRemoveEvent(eventName, handler);
            }
        }

        public void RemoveEvent<T1, T2, T3, T4, T5>(EventName eventName, EventDelegate<T1, T2, T3, T4, T5> handler)
        {
            if (_eventListeners.ContainsKey(eventName))
            {
                InternalRemoveEvent(eventName, handler);
            }
        }

        private void InternalRemoveEvent(EventName eventName, Delegate handler)
        {
            var del = _eventListeners[eventName];
            del = Delegate.Remove(del, handler);
            _eventListeners[eventName] = del;
            if (del == null)
            {
                _eventListeners.Remove(eventName);
            }
        }

        public void DispatchEvent(EventName eventName)
        {
            if (_eventListeners.TryGetValue(eventName, out Delegate handler))
            {
                ((EventDelegate) handler)();
            }
        }

        public void DispatchEvent<T1>(EventName eventName, T1 t1)
        {
            if (_eventListeners.TryGetValue(eventName, out Delegate handler))
            {
                ((EventDelegate<T1>) handler)(t1);
            }
        }

        public void DispatchEvent<T1, T2>(EventName eventName, T1 t1, T2 t2)
        {
            if (_eventListeners.TryGetValue(eventName, out Delegate handler))
            {
                ((EventDelegate<T1, T2>) handler)(t1, t2);
            }
        }

        public void DispatchEvent<T1, T2, T3>(EventName eventName, T1 t1, T2 t2, T3 t3)
        {
            if (_eventListeners.TryGetValue(eventName, out Delegate handler))
            {
                ((EventDelegate<T1, T2, T3>) handler)(t1, t2, t3);
            }
        }

        public void DispatchEvent<T1, T2, T3, T4, T5>(EventName eventName, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            if (_eventListeners.TryGetValue(eventName, out Delegate handler))
            {
                ((EventDelegate<T1, T2, T3, T4, T5>) handler)(t1, t2, t3, t4, t5);
            }
        }
    }
}