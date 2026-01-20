using System;

namespace IM.Storages
{
    public interface IStorageCell
    {
        IStorableReadOnly Item { get; set; }
        
        event Action<IStorableReadOnly, IStorableReadOnly> ItemChanged;
    }
}