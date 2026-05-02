using System;
using System.Collections.Generic;

namespace Core.Events
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> _subscribers = new();


        public static EventBinding<T> Subscribe<T>(Action<T> callback) where T : IEvent
        {
            Type eventType = typeof(T);

            if (!_subscribers.TryAdd(eventType, callback))
            {
                _subscribers[eventType] = Delegate.Combine(_subscribers[eventType], callback);
            }

            return new EventBinding<T>(callback, () => Unsubscribe(callback));
        }

        public static void Unsubscribe<T>(Action<T> callback) where T : IEvent
        {
            Type eventType = typeof(T);

            if (_subscribers.ContainsKey(eventType))
            {
                var currentDelegate = _subscribers[eventType];
                _subscribers[eventType] = Delegate.Remove(currentDelegate, callback);

                if (_subscribers[eventType] == null)
                {
                    _subscribers.Remove(eventType);
                }
            }
        }

        public static void Raise<T>(T eventData) where T : IEvent
        {
            Type eventType = typeof(T);

            if (_subscribers.TryGetValue(eventType, out Delegate callback))
            {
                (callback as Action<T>)?.Invoke(eventData);
            }
        }

        public static void Clear()
        {
            _subscribers.Clear();
        }
    }
}