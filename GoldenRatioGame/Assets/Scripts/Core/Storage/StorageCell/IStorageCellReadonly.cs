using System;

namespace IM.Storages
{
    public interface IStorageCellReadonly
    {
        IStorableReadOnly Item { get;  }
        IReadOnlyStorage Owner { get; }
        
        event Action<IStorableReadOnly, IStorableReadOnly> ItemChanged;
    }
}