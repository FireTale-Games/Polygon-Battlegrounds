using System;
using UnityEngine;

namespace FTS.Tools.ScriptableEvents
{
    public abstract class EventObserver<T> : ScriptableObject
    {
        protected Action<T> OnTrigger;

        public void AddObserver(Action<T> action) =>
            OnTrigger += action;

        public void RemoveObserver(Action<T> action) =>
            OnTrigger -= action;
    }

    public abstract class EventInvoker<T> : EventObserver<T>
    {
        public void Raise(T t) =>
            OnTrigger?.Invoke(t);
    }
}
