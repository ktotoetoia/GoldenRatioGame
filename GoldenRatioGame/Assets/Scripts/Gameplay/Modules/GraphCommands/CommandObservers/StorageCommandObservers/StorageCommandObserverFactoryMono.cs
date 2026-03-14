using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Storages;
using UnityEngine;

namespace IM.Modules
{
    public class StorageCommandObserverFactoryMono : MonoBehaviour, ICommandObserverAddFactory, ICommandObserverRemoveFactory
    {
        [SerializeField] private GameObject _contextSource;
        private IModuleEditingContext _context;

        private void Awake()
        {
            if(!_contextSource.TryGetComponent(out _context)) throw new NullReferenceException();
        }

        public ICommandObserver Create(IModule param1, ICollection<IModule> param2)
        {
            IStorableReadOnly storable = param1 as IStorableReadOnly ?? throw new ArgumentException();
            return new StorageCommandObserver(storable.Cell, storable);
        }

        public ICommandObserver Create(IModule param1, ICollection<IModule> param2, ICollection<IConnection> param3)
        {
            IStorableReadOnly storable = param1 as IStorableReadOnly ?? throw new ArgumentException();
            return new StorageCommandObserver(_context.Storage.FirstOrDefault(x => x.Item == null)??(_context.Storage as ICellFactoryStorage).CreateCell(), storable);
        }
    }
}