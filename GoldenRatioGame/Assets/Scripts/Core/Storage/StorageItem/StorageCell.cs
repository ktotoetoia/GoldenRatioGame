using System;

namespace IM.Storages
{
    public class StorageCell : IStorageCell
    {
        private IStorageItemSetter _item;

        public IStorageItem Item
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

                if (value is not IStorageItemSetter internalStorageItem)
                {
                    throw new ArgumentException("Item must implement IStorageItemSetter");
                }

                SetItem(internalStorageItem);
            }
        }

        private void SetItemToNull()
        {
            SetItemCellToNull();

            _item = null;
        }

        private void SetItemCellToNull()
        {
            if (_item != null)
            {
                _item.Cell = null;
            }
        }

        private void SetItem(IStorageItemSetter item)
        {
            SetItemCellToNull();
            
            _item = item;
            _item.Cell = this;
        }
    }
}