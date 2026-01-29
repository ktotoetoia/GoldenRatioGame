using IM.Abilities;
using IM.Entities;

namespace IM.Modules
{
    public interface IModuleEntity : IEntity
    {
        IModuleEditingContext ModuleEditingContext { get; }
        IAbilityPool AbilityPool { get; }
    }
}