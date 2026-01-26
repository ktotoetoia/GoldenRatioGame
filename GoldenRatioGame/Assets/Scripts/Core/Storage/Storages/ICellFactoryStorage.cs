using System.Collections;

namespace IM.Storages
{
    public interface ICellFactoryStorage : IItemMutableStorage
    {
        IStorableReadOnly ClearAndRemoveCell(IStorageCellReadonly cell);
        IStorageCellReadonly CreateCell();
        IStorageCellReadonly CreateCellAt(int index);
        void RemoveCell(IStorageCellReadonly cell);
    }
}