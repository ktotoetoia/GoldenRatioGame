using System;

namespace IM.Abilities
{
    public interface IUseContextAbility : IRequireAbilityUseContext
    {
        AbilityUseContext LastUsedContext { get; }
        event Action<AbilityUseContext> OnAbilityUsed;
    }
}