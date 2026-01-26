using System;

namespace IM.Storages
{
    public interface IStorageCellReadonly
    {
        IStorableReadOnly Item { get;  }
        IItemMutableStorage Owner { get; }
        
        event Action<IStorableReadOnly, IStorableReadOnly> ItemChanged;
    }
}