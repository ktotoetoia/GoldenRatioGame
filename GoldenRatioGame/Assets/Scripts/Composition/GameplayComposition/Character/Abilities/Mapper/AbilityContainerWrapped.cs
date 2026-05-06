using System;
using IM.Abilities;

namespace IM.Modules
{
    public sealed class AbilityContainerWrapped : IAbilityContainer
    {
        public IAbilityContainer Target { get; }
        public IAbilityReadOnly Ability => Target.Ability;
        
        public AbilityContainerWrapped(IAbilityContainer target)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
        }
    }
}