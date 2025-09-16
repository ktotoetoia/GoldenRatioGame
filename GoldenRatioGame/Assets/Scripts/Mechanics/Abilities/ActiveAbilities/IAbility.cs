using IM.Values;

namespace IM.Abilities
{
    public interface IAbility
    {
        ICooldownReadOnly Cooldown { get; }
        bool IsBeingUsed { get; }
        bool CanUse { get; }
    }
}