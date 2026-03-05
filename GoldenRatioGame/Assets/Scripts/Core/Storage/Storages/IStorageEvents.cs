using System;

namespace IM.Storages
{
    public interface IStorageEvents
    {
        event Action<int, int> CellsCountChanged;
        event Action<IStorageCellReadonly, IStorableReadOnly> ItemAdded;
        event Action<IStorageCellReadonly, IStorableReadOnly> ItemRemoved;
    }
}