using System;

namespace IM.Storages
{
    public interface IStorageCellReadonly
    {
        IStorableReadOnly Item { get;  }
        IStorage Owner { get; }
        
        event Action<IStorableReadOnly, IStorableReadOnly> ItemChanged;
    }
}