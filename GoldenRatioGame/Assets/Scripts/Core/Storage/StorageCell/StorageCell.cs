using System;

namespace IM.Storages
{
    public class StorageCell : IStorageCell
    {
        private IStorable _item;

        public event Action<IStorableReadOnly, IStorableReadOnly> ItemChanged;
        public IItemMutableStorage Owner { get; private set; }
        public IStorableReadOnly Item => _item;

        public StorageCell(IItemMutableStorage owner)
        {
            Owner = owner;
        }

        public void SetItem(IStorableReadOnly item)
        {
            if (_item == item)
            {
                return;
            }
                
            if (item is null)
            {
                SetItemToNull();
                return;
            }

            if (item is not IStorable internalStorageItem)
            {
                throw new ArgumentException("Item must implement IStorable");
            }

            SetItem(internalStorageItem);
        }

        private void SetItemToNull()
        {
            SetItemCellToNull();
            
            IStorable oldItem = _item;
            _item = null;
            
            ItemChanged?.Invoke(oldItem, _item);
        }

        private void SetItem(IStorable item)
        {
            SetItemCellToNull();
            
            IStorable oldItem = _item;
            _item = item;
            _item.Cell = this;
            
            ItemChanged?.Invoke(oldItem, _item);
        }

        private void SetItemCellToNull()
        {
            if (_item != null)
            {
                _item.Cell = null;
            }
        }
    }
}