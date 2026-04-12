using IM.LifeCycle;

namespace IM.Modules
{
    public interface IModuleEntity : IEntity
    {
        IModuleEditingContextEditor ModuleEditingContextEditor { get; }

        bool AddToContext(IExtensibleItem item);
        bool RemoveFromContext(IExtensibleItem item);
    }
}