using IM.Abilities;

namespace IM.Modules
{
    public interface IAbilityExtension : IExtension
    {
        IAbilityReadOnly Ability { get; }
    }
}