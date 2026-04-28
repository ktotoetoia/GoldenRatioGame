using IM.LifeCycle;

namespace IM.Modules
{
    public interface IModuleEditingContext : IModuleEditingContextReadOnly
    {
        ITypeRegistry<IEditingService> Services { get; }
        
        IGraphEditingService<IExtensibleItem> GraphEditing { get; }
        IGraphEditingService<IExtensibleItem> UnsafeGraphEditing { get; }
        IStorageEditingService StorageEditing { get; }
        bool AddService(IEditingService service);
        bool AddCapability(object capability);
    }
}