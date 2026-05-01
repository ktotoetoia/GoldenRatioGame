using IM.Items;
using IM.Storages;

namespace IM.Modules
{
    public class StorageEditingService : IStorageEditingService
    {
        private readonly ICellFactoryStorage _storage;

        public IReadOnlyStorage Storage => _storage;
        
        public StorageEditingService(ICellFactoryStorage storage)
        {
            _storage = storage;
        }
        
        public bool AddToStorage(IItem item)
        {
            if (item is not IStorable storable || _storage.ContainsItem(storable)) return false;
            
            _storage.SetItemToFirstOrNew(storable);

            return true;
        }

        public bool RemoveFromStorage(IItem item)
        {
            if (item is not IStorable storable || !_storage.ContainsItem(storable)) return false;
            
            _storage.ClearCell(_storage.GetCell(storable));

            return true;
        }
    }
}