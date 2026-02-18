using System;
using System.Collections.Generic;

namespace IM.Events
{
    public sealed class ValueStorageContainer : IValueStorageContainer
    {
        private readonly Dictionary<Type, object> _map = new();

        public IValueStorage<T> Get<T>()
        {
            return (IValueStorage<T>)_map[typeof(T)];
        }

        public bool TryGet<T>(out IValueStorage<T> storage)
        {
            if (_map.TryGetValue(typeof(T), out var obj))
            {
                storage = (IValueStorage<T>)obj;
                return true;
            }

            storage = null;
            return false;
        }

        public IValueStorage<T> GetOrCreate<T>()
        {
            if (TryGet<T>(out var storage))
                return storage;

            storage = new ValueStorage<T>();
            _map[typeof(T)] = storage;
            return storage;
        }

        public void Remove<T>()
        {
            _map.Remove(typeof(T));
        }
    }
}