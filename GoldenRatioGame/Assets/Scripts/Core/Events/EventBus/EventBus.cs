using System;
using System.Collections.Generic;
using TDS.Handlers;

namespace TDS.Events
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<object>> _handlers = new();

        public void Subscribe<T>(IHandler<T> handler)
        {
            Type type = typeof(T);
            if (!_handlers.TryGetValue(type, out List<object> list))
            {
                list = new List<object>();
                _handlers[type] = list;
            }
            list.Add(handler);
        }

        public void Unsubscribe<T>(IHandler<T> handler)
        {
            Type type = typeof(T);
            if (_handlers.TryGetValue(type, out List<object> list))
            {
                list.Remove(handler);
                if (list.Count == 0)
                    _handlers.Remove(type);
            }
        }

        public void Publish<T>(T evt)
        {
            Type type = typeof(T);
            
            if (_handlers.TryGetValue(type, out List<object> list))
            {
                object[] snapshot = list.ToArray();
                foreach (object obj in snapshot)
                {
                    ((IHandler<T>)obj).Handle(evt);
                }
            }
        }
    }
}