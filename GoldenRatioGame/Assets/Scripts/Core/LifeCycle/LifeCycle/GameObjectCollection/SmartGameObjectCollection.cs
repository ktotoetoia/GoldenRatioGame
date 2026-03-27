using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM.LifeCycle
{
    public class SmartGameObjectCollection : ICollection<GameObject>, IReadOnlyCollection<GameObject>
    {
        private readonly List<GameObject> _objects = new();
        public bool IsReadOnly => false;

        public int Count
        {
            get
            {
                _objects.RemoveAll(item => !item);
                return _objects.Count;
            }
        }

        public SmartGameObjectCollection()
        {
            
        }

        public SmartGameObjectCollection(IEnumerable<GameObject>  source)
        {
            foreach (GameObject gameObject in source) Add(gameObject);
        }
        
        public void Add(GameObject item)
        {
            if (item)
            {
                _objects.Add(item);
            }
        }

        public bool Remove(GameObject item) => _objects.Remove(item);
        public void Clear() => _objects.Clear();
        public bool Contains(GameObject item) => _objects.Contains(item);

        public void CopyTo(GameObject[] array, int arrayIndex)
        {
            _objects.RemoveAll(item => !item);
            _objects.CopyTo(array, arrayIndex);
        }

        public IEnumerator<GameObject> GetEnumerator()
        {
            for (int i = _objects.Count - 1; i >= 0; i--)
            {
                GameObject go = _objects[i];

                if (!go)
                {
                    _objects.RemoveAt(i);
                    continue;
                }

                yield return go;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public GameObject Get(int index)
        {
            if (index < 0 || index >= _objects.Count) return null;

            GameObject go = _objects[index];

            if (!go)
            {
                _objects.RemoveAt(index);
                return null;
            }

            return go;
        }

        public GameObject this[int index] => Get(index);
    }
}