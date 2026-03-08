using System;
using System.Collections.Generic;
using IM.Graphs;
using IM.Storages;

namespace IM.Modules
{
    public class StorageCommandObserverAddFactory : ICommandObserverAddFactory
    {
        public ICommandObserver Create(IModule param1, ICollection<IModule> param2)
        {
            IStorableReadOnly storable = param1 as IStorableReadOnly ?? throw new ArgumentException();
            return new StorageCommandObserver(storable.Cell, storable);
        }
    }
}