using System;
using System.Collections.Generic;

namespace Common
{
    public static class EventManager
    {
        private static readonly Dictionary<Type, object> s_Events = new Dictionary<Type, object>();
        
        public static void Subscribe<T>(Action<T> handler)
        {
            var eventType = typeof(T);
            if (!s_Events.ContainsKey(eventType))
            {
                s_Events.Add(eventType, new ActionWrapper<T>());
            }
            
            ((ActionWrapper<T>) s_Events[eventType]).Action += handler;
        }

        public static void Unsubscribe<T>(Action<T> handler)
        {
            var eventType = typeof(T);
            if (!s_Events.ContainsKey(eventType))
            {
                return;
            }
            
            ((ActionWrapper<T>) s_Events[eventType]).Action -= handler;
        }
        
        public static void Send<T>(T eventData)
        {
            var eventType = typeof(T);
            if (!s_Events.ContainsKey(eventType))
            {
                return;
            }
            
            ((ActionWrapper<T>) s_Events[eventType]).Invoke(eventData);
        }
    }
}