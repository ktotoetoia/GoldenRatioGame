using System.Collections.Generic;
using IM.LifeCycle;
using IM.Modules;
using UnityEngine;
using UnityEngine.Pool;

namespace IM.UI
{
    public class GameObjectStatusDisplayCollection : MonoBehaviour, IGameObjectStatusDisplayCollection
    {
        [SerializeField] private GameObject _displayPrefab;
        private readonly EntityCollection _entityCollection = new();
        private readonly Dictionary<GameObject,IGameObjectStatusDisplay> _assigned= new();
        private ObjectPool<IGameObjectStatusDisplay> _pool;
        
        public IEnumerable<GameObject> Showed => _assigned.Keys;
        
        private void Awake()
        {
            _pool = new ObjectPool<IGameObjectStatusDisplay>(CreateDisplay);

            _entityCollection.EntityAdded += x => InternalAdd(x.GameObject);
            _entityCollection.EntityRemoved += x => InternalRemove(x.GameObject);
        }

        private IGameObjectStatusDisplay CreateDisplay()
        {
            GameObject go = Instantiate(_displayPrefab, transform);

            return go.GetComponent<IGameObjectStatusDisplay>();
        }

        public void Add(GameObject go)
        {
            if(!go.TryGetComponent(out IEntity entity))return;

            _entityCollection.Add(entity);
        }

        public void Remove(GameObject go)
        {
            if(!go ||!go.TryGetComponent(out IEntity entity))return;
            
            _entityCollection.Remove(entity);
        }

        private void InternalAdd(GameObject go)
        {
            if(_assigned.ContainsKey(go)) return;
            
            IGameObjectStatusDisplay display = _pool.Get();
            _assigned[go] = display;
            display.Displayed = go;
        }

        private void InternalRemove(GameObject go)
        {
            if (!_assigned.Remove(go, out var display)) return;
            
            display.Displayed = null;
            _pool.Release(display);
        }
    }
}