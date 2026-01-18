using IM.Abilities;

namespace IM.Modules
{
    public interface IAbilityExtension : IModuleExtension
    {
        IAbility Ability { get; }
    }
}