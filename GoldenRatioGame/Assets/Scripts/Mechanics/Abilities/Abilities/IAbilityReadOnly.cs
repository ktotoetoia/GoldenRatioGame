using IM.Values;

namespace IM.Abilities
{
    public interface IAbilityReadOnly
    {
        ICooldownReadOnly Cooldown { get; }
        bool CanUse { get; }
    }
}