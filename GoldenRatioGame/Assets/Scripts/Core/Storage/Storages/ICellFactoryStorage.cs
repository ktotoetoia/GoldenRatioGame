namespace IM.Storages
{
    public interface ICellFactoryStorage : IStorage
    {
        IStorableReadOnly ClearAndRemoveCell(IStorageCellReadonly cell);
        IStorageCellReadonly CreateCell();
        IStorageCellReadonly FirstOrNew();
        IStorageCellReadonly SetItemToFirstOrNew(IStorableReadOnly item);
        IStorageCellReadonly CreateCellAt(int index);
        void RemoveCell(IStorageCellReadonly cell);
    }
}