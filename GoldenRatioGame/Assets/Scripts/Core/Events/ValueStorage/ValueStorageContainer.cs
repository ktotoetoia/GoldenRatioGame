using System;
using System.Collections.Generic;

namespace IM.Events
{
    public sealed class ValueStorageContainer : IValueStorageContainer
    {
        private readonly Dictionary<(Type, string), object> _map = new();

        private static (Type, string) Key<T>(string tag)
            => (typeof(T), tag);

        public IValueStorage<T> Get<T>(string tag = null)
        {
            return (IValueStorage<T>)_map[Key<T>(tag)];
        }

        public bool TryGet<T>(out IValueStorage<T> storage, string tag = null)
        {
            if (_map.TryGetValue(Key<T>(tag), out var obj))
            {
                storage = (IValueStorage<T>)obj;
                return true;
            }

            storage = null;
            return false;
        }

        public IValueStorage<T> GetOrCreate<T>(string tag = null)
        {
            if (TryGet<T>(out var storage, tag))
                return storage;

            storage = new ValueStorage<T>();
            _map[Key<T>(tag)] = storage;
            return storage;
        }

        public void Remove<T>(string tag = null)
        {
            _map.Remove(Key<T>(tag));
        }
    }
}