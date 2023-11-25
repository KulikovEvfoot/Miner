using System;
using System.Collections.Generic;

namespace Core.EventProducer.Runtime
{
    public class EventProducer<T> : IEventProducer<T>
    {
        private readonly List<T> m_Observers = new List<T>();
        
        public void Attach(T observer)
        {
            if (m_Observers.Contains(observer))
            {
                return;
            }
            
            m_Observers.Add(observer);
        }

        public void Detach(T observer)
        {
            m_Observers.Remove(observer);
        }

        public void NotifyAll(Action<T> action)
        {
            var temp = new List<T>(m_Observers);
            foreach (var observer in temp)
            {
                action?.Invoke(observer);
            }
        }
    }
}