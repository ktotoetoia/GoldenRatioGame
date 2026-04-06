using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM.LifeCycle
{
    public class SmartGameObjectCollectionMono :MonoBehaviour, ICollection<GameObject>, IReadOnlyCollection<GameObject>
    {
        private readonly SmartCollection<GameObject> _smartCollection = new();
        public int Count => _smartCollection.Count;
        public bool IsReadOnly => _smartCollection.IsReadOnly;
        
        public IEnumerator<GameObject> GetEnumerator() => _smartCollection.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_smartCollection).GetEnumerator();
        public void Add(GameObject item) => _smartCollection.Add(item);
        public void Clear() => _smartCollection.Clear();
        public bool Contains(GameObject item) => _smartCollection.Contains(item);
        public void CopyTo(GameObject[] array, int arrayIndex) => _smartCollection.CopyTo(array, arrayIndex);
        public bool Remove(GameObject item) => _smartCollection.Remove(item);
    }
}