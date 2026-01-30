namespace IM.Storages
{
    public interface IStorage : IReadOnlyStorage
    {
        void SetItem(IStorageCellReadonly cell, IStorableReadOnly item);
        IStorableReadOnly ClearCell(IStorageCellReadonly cell);
    }
}