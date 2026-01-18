using System;
using System.Collections;
using System.Collections.Generic;

namespace IM.Storages
{
    public interface ICellFactoryStorage : IItemMutableStorage
    {
        IStorageItem ClearCell(IStorageCell cell);
        IStorageCell CreateCellAt(int index);
    }

    public interface IItemMutableStorage : IReadOnlyStorage
    {
        event Action<int, int> CellsCountChanged;
        event Action<IStorageCell, IStorageItem> ItemAdded;
        event Action<IStorageCell, IStorageItem> ItemRemoved;

        void SetItem(IStorageCell cell, IStorageItem item);
    }

    public interface ICellListStorage : IItemMutableStorage, IList<IStorageCell>, IList
    {
        
    }
}