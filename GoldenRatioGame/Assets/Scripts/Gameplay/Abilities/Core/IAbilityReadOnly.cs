using IM.Common;
using IM.Items;

namespace IM.Abilities
{
    public interface IAbilityReadOnly : IHaveName, IHaveDescription
    {
        ICooldownReadOnly Cooldown { get; }
        bool CanUse { get; }
    }
}