using System;

namespace Core.Events
{
    public class EventBinding<T> : IDisposable where T : IEvent
    {
        private Action<T> _onEvent;
        private Action _onDispose;
        
        public EventBinding(Action<T> onEvent, Action onDispose)
        {
            _onEvent = onEvent;
            _onDispose = onDispose;
        }


        public void Dispose()
        {
            _onDispose?.Invoke();
            _onEvent = null;
            _onDispose = null;
        }
    }
}