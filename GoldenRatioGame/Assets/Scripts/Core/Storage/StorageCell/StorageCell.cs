using System;

namespace IM.Storages
{
    public class StorageCell : IStorageCell
    {
        private IStorable _item;
        public event Action<IStorableReadOnly, IStorableReadOnly> ItemChanged;

        public IStorableReadOnly Item
        {
            get => _item;
            set
            {
                if (_item == value)
                {
                    return;
                }

                if (value is null)
                {
                    SetItemToNull();
                    return;
                }

                if (value is not IStorable internalStorageItem)
                {
                    throw new ArgumentException("Item must implement IStorageItemSetter");
                }

                SetItem(internalStorageItem);
            }
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