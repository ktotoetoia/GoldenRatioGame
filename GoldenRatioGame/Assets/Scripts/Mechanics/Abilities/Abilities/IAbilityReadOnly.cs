using IM.Modules;
using IM.Values;

namespace IM.Abilities
{
    public interface IAbilityReadOnly
    {
        ITypeRegistry<IAbilityDescriptor> AbilityDescriptorsRegistry { get; }
        ICooldownReadOnly Cooldown { get; }
        bool CanUse { get; }
    }
}