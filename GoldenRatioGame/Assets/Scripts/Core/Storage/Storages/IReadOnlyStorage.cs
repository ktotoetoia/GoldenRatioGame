using System;
using System.Collections;
using System.Collections.Generic;

namespace IM.Storages
{
    public interface IReadOnlyStorage : IReadOnlyList<IStorageCellReadonly>
    {
        event Action<int, int> CellsCountChanged;
        event Action<IStorageCellReadonly, IStorableReadOnly> ItemAdded;
        event Action<IStorageCellReadonly, IStorableReadOnly> ItemRemoved;

        bool Contains(IStorageCellReadonly cell);
        int IndexOf(IStorageCellReadonly cell);
        
        bool ContainsItem(IStorableReadOnly item);
        IStorageCellReadonly GetCell(IStorableReadOnly item);

        IList GetListForUI();
    }
}