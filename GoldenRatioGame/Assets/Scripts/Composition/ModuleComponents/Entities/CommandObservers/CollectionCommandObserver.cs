using System.Collections.Generic;

namespace IM.Modules
{
    public class CollectionCommandObserver<T> : ICommandObserver
    {
        private readonly ICollection<T> _collection;
        private readonly T _obj;

        public CollectionCommandObserver(ICollection<T> collection, T obj)
        {
            _collection = collection;
            _obj = obj;
        }

        public void OnModuleAdded()
        {
            _collection.Add(_obj);
        }

        public void OnModuleRemoved()
        {
            _collection.Remove(_obj);
        }
    }
}