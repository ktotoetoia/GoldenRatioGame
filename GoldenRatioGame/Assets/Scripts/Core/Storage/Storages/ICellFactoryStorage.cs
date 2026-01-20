using System.Collections;

namespace IM.Storages
{
    public interface ICellFactoryStorage : IItemMutableStorage
    {
        IStorableReadOnly ClearCell(IStorageCell cell);
        IStorageCell CreateCellAt(int index);
    }
}