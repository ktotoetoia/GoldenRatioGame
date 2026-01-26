namespace IM.Storages
{
    public interface IStorageCell : IStorageCellReadonly
    {
        void SetItem(IStorableReadOnly item);
    }
}