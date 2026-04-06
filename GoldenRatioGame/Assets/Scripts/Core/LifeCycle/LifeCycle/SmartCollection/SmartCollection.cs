using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM.LifeCycle
{
    /// <summary>
    /// A collection that automatically removes destroyed Unity objects.
    /// Works with GameObjects, Components, and Interfaces implemented by MonoBehaviours.
    /// </summary>
    public class SmartCollection<T> : ICollection<T>, IReadOnlyCollection<T>
    {
        protected readonly List<T> _items = new();
        public bool IsReadOnly => false;

        public int Count
        {
            get
            {
                Prune();
                return _items.Count;
            }
        }

        public SmartCollection() { }

        public SmartCollection(IEnumerable<T> source)
        {
            foreach (var item in source) Add(item);
        }

        public void Add(T item)
        {
            if (IsAlive(item)) _items.Add(item);
        }

        public bool Remove(T item) => _items.Remove(item);
        public void Clear() => _items.Clear();
        public bool Contains(T item) => IsAlive(item) && _items.Contains(item);

        public void CopyTo(T[] array, int arrayIndex)
        {
            Prune();
            _items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                if (!IsAlive(_items[i]))
                {
                    _items.RemoveAt(i);
                    continue;
                }
                yield return _items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _items.Count) return default;
                
                T item = _items[index];
                if (!IsAlive(item))
                {
                    _items.RemoveAt(index);
                    return default;
                }
                return item;
            }
        }

        /// <summary>
        /// Removes all null or destroyed Unity objects from the list.
        /// </summary>
        public void Prune() => _items.RemoveAll(item => !IsAlive(item));

        private static bool IsAlive(T item)
        {
            return item != null && (item as Object ?? true);
        }
    }
}