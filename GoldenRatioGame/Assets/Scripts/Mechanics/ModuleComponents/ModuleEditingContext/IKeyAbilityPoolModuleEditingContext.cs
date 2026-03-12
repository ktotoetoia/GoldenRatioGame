using IM.Abilities;

namespace IM.Modules
{
    public interface IKeyAbilityPoolModuleEditingContext : IModuleEditingContext
    {
        public IKeyAbilityPool KeyAbilityPool { get; }
    }
}