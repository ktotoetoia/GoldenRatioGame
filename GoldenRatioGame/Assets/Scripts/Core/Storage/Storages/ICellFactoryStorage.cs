using System.Collections;

namespace IM.Storages
{
    public interface ICellFactoryStorage : IItemMutableStorage
    {
        IStorableReadOnly ClearAndRemoveCell(IStorageCell cell);
        IStorageCell CreateCell();
        IStorageCell CreateCellAt(int index);
        void RemoveCell(IStorageCell cell);
    }
}