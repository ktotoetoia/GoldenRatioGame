using IM.Items;
using IM.Storages;

namespace IM.Modules
{
    public interface IStorageEditingService : IEditingService
    {
        IReadOnlyStorage Storage { get; } 
        
        bool AddToStorage(IItem item);
        bool RemoveFromStorage(IItem item);
    }
}