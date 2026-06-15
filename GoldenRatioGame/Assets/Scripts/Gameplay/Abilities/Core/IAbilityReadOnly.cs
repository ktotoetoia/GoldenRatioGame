using IM.Items;
using IM.Values;

namespace IM.Abilities
{
    public interface IAbilityReadOnly : IHaveName, IHaveDescription
    {
        float WindUpTime { get; }
        ICooldownReadOnly Cooldown { get; }
        bool CanUse { get; }
    }
}