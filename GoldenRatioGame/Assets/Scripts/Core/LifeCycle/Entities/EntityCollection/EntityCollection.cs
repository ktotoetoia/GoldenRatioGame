using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IM.LifeCycle
{
    public sealed class EntityCollection : IEntityCollection, IDisposable
    {
        private readonly HashSet<IEntity> _entities = new();

        public int Count => _entities.Count;

        public event Action<IEntity> EntityAdded;
        public event Action<IEntity> EntityRemoved;
        public event Action<IEntity> EntityDestroyed;
        public event Action CollectionChanged;

        public bool Add(IEntity entity)
        {
            if (entity == null) return false;
            if (!_entities.Add(entity)) return false;

            entity.Destroyed += HandleInternalDestruction;
        
            EntityAdded?.Invoke(entity);
            CollectionChanged?.Invoke();
            return true;
        }

        public bool Remove(IEntity entity)
        {
            if (entity == null || !_entities.Remove(entity)) return false;

            entity.Destroyed -= HandleInternalDestruction;
        
            EntityRemoved?.Invoke(entity);
            CollectionChanged?.Invoke();
            return true;
        }

        private void HandleInternalDestruction(IEntity entity)
        {    
            entity.Destroyed -= HandleInternalDestruction;
            
            if (_entities.Remove(entity))
            {
                EntityDestroyed?.Invoke(entity);
                EntityRemoved?.Invoke(entity);
                CollectionChanged?.Invoke();
            }
        }

        public void Clear()
        {
            IEntity[] snapshot = _entities.ToArray();
            foreach (IEntity entity in snapshot)
            {
                entity.Destroyed -= HandleInternalDestruction;
                EntityRemoved?.Invoke(entity);
            }
            _entities.Clear();
            CollectionChanged?.Invoke();
        }

        public void Dispose() => Clear();

        public IEnumerator<IEntity> GetEnumerator() => _entities.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
        public bool Contains(IEntity entity) => _entities.Contains(entity);
    }
}