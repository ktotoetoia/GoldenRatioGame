using System;
using IM.Storages;

namespace IM.Modules
{
    public class StorageCommandObserver : ICommandObserver
    {
        private readonly IStorage _storage;
        private readonly IStorableReadOnly _storable;
        private readonly IStorageCellReadonly _cell;
        
        public StorageCommandObserver(IStorageCellReadonly cell, IStorableReadOnly storable)
        {
            _cell = cell ?? throw new ArgumentNullException(nameof(cell));
            _storage = cell.Owner ?? throw new InvalidOperationException("Cell has no owner.");
            _storable = storable ?? throw new ArgumentNullException(nameof(storable));
        }

        public void OnModuleAdded()
        {
            _storage.ClearCell(_cell);
        }

        public void OnModuleRemoved()
        {
            _storage.SetItem(_cell,_storable);
        }
    }
}