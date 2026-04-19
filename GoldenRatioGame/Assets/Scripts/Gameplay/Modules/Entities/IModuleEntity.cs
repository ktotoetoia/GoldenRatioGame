using IM.Items;
using IM.LifeCycle;

namespace IM.Modules
{
    public interface IModuleEntity : IEntity
    {
        IModuleEditingContextEditor ModuleEditingContextEditor { get; }

        bool AddToContext(IItem item);
        bool RemoveFromContext(IItem item);
    }
}