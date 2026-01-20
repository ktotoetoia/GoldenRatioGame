namespace IM.Storages
{
    public interface IStorable : IStorableReadOnly
    {
        new IStorageCell Cell { get; set; }
    }
}