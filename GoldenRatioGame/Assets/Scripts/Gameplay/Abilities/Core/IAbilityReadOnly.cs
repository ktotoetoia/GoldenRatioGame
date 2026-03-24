using IM.Items;
using IM.Values;

namespace IM.Abilities
{
    public interface IAbilityReadOnly : IHaveName, IHaveDescription
    {
        ICooldownReadOnly Cooldown { get; }
        bool CanUse { get; }
    }
}