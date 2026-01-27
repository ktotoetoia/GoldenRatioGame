using IM.Abilities;
using IM.Entities;

namespace IM.Modules
{
    public interface IModuleEntity : IEntity
    {
        IModuleController ModuleController { get; }
        IAbilityPool AbilityPool { get; }
        
    }
}