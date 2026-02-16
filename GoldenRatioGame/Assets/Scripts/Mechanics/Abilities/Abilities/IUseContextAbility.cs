using System;

namespace IM.Abilities
{
    public interface IUseContextAbility : IAbility, IRequireAbilityUseContext
    {
        AbilityUseContext LastUsedContext { get; }
        event Action<AbilityUseContext> OnAbilityUsed;
    }
}