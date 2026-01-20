using System;
using System.Collections;

namespace IM.Storages
{
    public interface IItemMutableStorage : IReadOnlyStorage
    {
        event Action<int, int> CellsCountChanged;
        event Action<IStorageCell, IStorableReadOnly> ItemAdded;
        event Action<IStorageCell, IStorableReadOnly> ItemRemoved;

        void SetItem(IStorageCell cell, IStorableReadOnly item);

        IList GetListForUI();
    }
}