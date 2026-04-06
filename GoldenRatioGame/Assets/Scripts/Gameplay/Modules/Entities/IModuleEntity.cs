using IM.LifeCycle;

namespace IM.Modules
{
    public interface IModuleEntity : IEntity
    {
        IModuleEditingContext ModuleEditingContext { get; }
    }
}