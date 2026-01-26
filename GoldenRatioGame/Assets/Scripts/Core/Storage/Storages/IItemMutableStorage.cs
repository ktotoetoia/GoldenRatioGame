using System;
using System.Collections;

namespace IM.Storages
{
    public interface IItemMutableStorage : IReadOnlyStorage
    {
        event Action<int, int> CellsCountChanged;
        event Action<IStorageCellReadonly, IStorableReadOnly> ItemAdded;
        event Action<IStorageCellReadonly, IStorableReadOnly> ItemRemoved;

        void SetItem(IStorageCellReadonly cell, IStorableReadOnly item);
        IStorableReadOnly ClearCell(IStorageCellReadonly cell);

        IList GetListForUI();
    }
}