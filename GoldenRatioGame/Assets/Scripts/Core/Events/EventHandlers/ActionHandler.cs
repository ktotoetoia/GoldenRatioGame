using System;

namespace TDS.Handlers
{
    public class ActionHandler<T> : IHandler<T>
    {
        private readonly Action<T> _handler;
        private readonly Func<T, bool> _canHandle;
        public ActionHandler(Action<T> handler) : this(handler, x => true)
        {
            
        }

        public ActionHandler(Action<T> handler, Func<T, bool> canHandle)
        {
            _handler = handler;
            _canHandle = canHandle;
        }

        public bool CanHandle(T operation)
        {
            return _canHandle(operation);
        }

        public void Handle(T operation)
        {
            if (_canHandle(operation))
            {
                _handler(operation);
            }
        }
    }
}