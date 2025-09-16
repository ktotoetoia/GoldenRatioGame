using IM.Economy;

namespace IM.Abilities
{
    public interface IAbility
    {
        ICooldownReadOnly Cooldown { get; }
        
        bool TryUse();
        bool CanUse();
    }
}