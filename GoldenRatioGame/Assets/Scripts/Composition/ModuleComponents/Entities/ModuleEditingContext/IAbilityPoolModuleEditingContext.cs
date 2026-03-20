using IM.Abilities;

namespace IM.Modules
{
    public interface IAbilityPoolModuleEditingContext : IModuleEditingContext
    {
        public IAbilityPool KeyAbilityPool { get; }
    }
}