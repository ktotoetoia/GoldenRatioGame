using System.Linq;
using IM.Items;
using IM.Storages;

namespace IM.Modules
{
    public class StorageEditingService : IStorageEditingService
    {
        private readonly ICellFactoryStorage _storage;

        public StorageEditingService(ICellFactoryStorage storage)
        {
            _storage = storage;
        }

        public bool AddToStorage(IItem item)
        {
            if (item.ItemState == ItemState.Hide|| item is not IStorable storable || _storage.ContainsItem(storable)) return false;
            
            _storage.SetItem(_storage.FirstOrDefault(x => x.Item == null) ?? _storage.CreateCell(), storable);
            item.ItemState = ItemState.Hide;

            return true;
        }

        public bool RemoveFromContext(IItem item)
        {
            if (  item is not IStorable storable || !_storage.ContainsItem(storable)) return false;
            
            _storage.ClearCell(_storage.GetCell(storable));
            item.ItemState = ItemState.Show;

            return true;
        }
    }
}