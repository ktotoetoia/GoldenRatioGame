using System;

namespace IM.Abilities
{
    public interface IUseContextAbility : IInstantAbility, IRequireAbilityUseContext
    {
        AbilityUseContext LastUsedContext { get; }
        event Action<AbilityUseContext> OnAbilityUsed;
    }
}