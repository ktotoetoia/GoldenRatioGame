using IM.Items;

namespace IM.Modules
{
    public interface IStorageEditingService : IEditingService
    {
        bool AddToStorage(IItem item);
        bool RemoveFromContext(IItem item);
    }
}