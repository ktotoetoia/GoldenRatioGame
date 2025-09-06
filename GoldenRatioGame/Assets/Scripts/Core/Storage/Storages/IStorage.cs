namespace IM.Storages
{
    public interface IStorage : IReadOnlyStorage
    {
        void SetItem(IStorageCell cell, IStorageItem item);
        IStorageItem EmptyCell(IStorageCell cell);
        IStorageCell CreateAndInsertCell(int index);
    }
}