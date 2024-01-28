using System;

namespace Common
{
    public class ActionWrapper<T>
    {
        public event Action<T> Action;

        public void Invoke(T data)
        {
            Action?.Invoke(data);
        }
    }

}