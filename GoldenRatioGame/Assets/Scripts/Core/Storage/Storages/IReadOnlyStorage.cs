using System.Collections.Generic;

namespace IM.Storages
{
    public interface IReadOnlyStorage : IReadOnlyList<IStorageCell>
    {
        bool Contains(IStorageCell cell);
        int IndexOf(IStorageCell cell);
    }
}