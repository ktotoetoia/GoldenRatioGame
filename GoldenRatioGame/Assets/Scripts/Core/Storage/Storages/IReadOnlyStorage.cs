using System.Collections.Generic;

namespace IM.Storages
{
    public interface IReadOnlyStorage : IReadOnlyList<IStorageCellReadonly>
    {
        bool Contains(IStorageCellReadonly cell);
        int IndexOf(IStorageCellReadonly cell);
        
        bool ContainsItem(IStorableReadOnly item);
        IStorageCellReadonly GetCell(IStorableReadOnly item);
    }
}