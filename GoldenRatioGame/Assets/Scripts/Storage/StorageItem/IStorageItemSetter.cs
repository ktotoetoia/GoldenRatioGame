namespace IM.Storages
{
    public interface IStorageItemSetter : IStorageItem
    {
        new IStorageCell Cell { get; set; }
    }
}