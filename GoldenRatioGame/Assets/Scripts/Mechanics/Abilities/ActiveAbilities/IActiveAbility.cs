using IM.Values;

namespace IM.Abilities
{
    public interface IActiveAbility
    {
        ICooldownReadOnly Cooldown { get; }
        
        bool TryUse();
        bool CanUse();
    }
}