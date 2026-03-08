using System;
using System.Collections.Generic;
using IM.Graphs;
using IM.Storages;

namespace IM.Modules
{
    public class StorageCommandObserverRemoveFactory : ICommandObserverRemoveFactory
    {
        private readonly Func<IModule, IStorageCellReadonly> _getStorageCell;

        public StorageCommandObserverRemoveFactory(Func<IModule, IStorageCellReadonly> getStorageCell)
        {
            _getStorageCell = getStorageCell;
        }

        public ICommandObserver Create(IModule param1, ICollection<IModule> param2, ICollection<IConnection> param3)
        {
            IStorableReadOnly storable = param1 as IStorableReadOnly ?? throw new ArgumentException();
            return new StorageCommandObserver(_getStorageCell(param1), storable);
        }
    }
}