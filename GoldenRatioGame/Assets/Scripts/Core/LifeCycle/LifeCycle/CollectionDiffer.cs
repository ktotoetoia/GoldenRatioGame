using System;
using System.Collections.Generic;
using System.Linq;

namespace IM.Modules
{
    public class CollectionDiffer<T>
    {
        private readonly Action<T> _onAdded;
        private readonly Action<T> _onRemoved;
        private List<T> _current = new();

        public CollectionDiffer(Action<T> onAdded, Action<T> onRemoved)
        {
            _onAdded = onAdded;
            _onRemoved = onRemoved;
        }

        public void Update(IEnumerable<T> newCollection)
        {
            var newList = newCollection.ToList();
        
            foreach (T item in newList.Except(_current)) _onAdded?.Invoke(item);
            foreach (T item in _current.Except(newList)) _onRemoved?.Invoke(item);
        
            _current = newList;
        }
    }
}