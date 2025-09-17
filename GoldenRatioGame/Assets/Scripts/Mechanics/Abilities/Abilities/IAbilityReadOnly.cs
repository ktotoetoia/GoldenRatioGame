using IM.Values;

namespace IM.Abilities
{
    public interface IAbilityReadOnly
    {
        ICooldownReadOnly Cooldown { get; }
        bool IsBeingUsed { get; }
        bool CanUse { get; }
    }
}