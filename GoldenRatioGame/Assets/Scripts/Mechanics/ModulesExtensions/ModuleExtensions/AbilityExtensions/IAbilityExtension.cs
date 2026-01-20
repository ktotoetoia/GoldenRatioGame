using IM.Abilities;

namespace IM.Modules
{
    public interface IAbilityExtension : IExtension
    {
        IAbility Ability { get; }
    }
}