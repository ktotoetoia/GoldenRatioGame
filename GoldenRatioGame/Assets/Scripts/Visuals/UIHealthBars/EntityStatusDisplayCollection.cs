using System.Collections.Generic;
using IM.LifeCycle;
using UnityEngine;
using UnityEngine.Pool;

namespace IM.UI
{
    public class EntityStatusDisplayCollection : MonoBehaviour, IGameObjectStatusDisplayCollection
    {
        [SerializeField] private GameObject _displayPrefab;
        private readonly EntityCollection _displayedEntities = new();
        private readonly Dictionary<GameObject,IGameObjectStatusDisplay> _assignedDisplays = new();
        private ObjectPool<IGameObjectStatusDisplay> _pool;
        
        public IEnumerable<GameObject> Displayed => _assignedDisplays.Keys;
        
        private void Awake()
        {
            _pool = new ObjectPool<IGameObjectStatusDisplay>(CreateDisplay);

            _displayedEntities.EntityAdded += x => InternalAdd(x.GameObject);
            _displayedEntities.EntityRemoved += x => InternalRemove(x.GameObject);
        }

        private IGameObjectStatusDisplay CreateDisplay()
        {
            GameObject go = Instantiate(_displayPrefab, transform);

            return go.GetComponent<IGameObjectStatusDisplay>();
        }

        public void Add(GameObject go)
        {
            if(!go.TryGetComponent(out IEntity entity)) return;

            _displayedEntities.Add(entity);
        }

        public void Remove(GameObject go)
        {
            if(!go ||!go.TryGetComponent(out IEntity entity)) return;
            
            _displayedEntities.Remove(entity);
        }

        private void InternalAdd(GameObject go)
        {
            if(_assignedDisplays.ContainsKey(go)) return;
            
            IGameObjectStatusDisplay display = _pool.Get();
            _assignedDisplays[go] = display;
            display.Displayed = go;
        }

        private void InternalRemove(GameObject go)
        {
            if (!_assignedDisplays.Remove(go, out var display)) return;
            
            display.Displayed = null;
            _pool.Release(display);
        }
    }
}