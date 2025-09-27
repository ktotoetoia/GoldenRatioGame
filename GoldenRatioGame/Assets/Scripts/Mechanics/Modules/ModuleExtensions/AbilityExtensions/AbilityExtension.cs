using IM.Abilities;

namespace IM.Modules
{
    public class AbilityExtension : IAbilityExtension
    {
        public IAbility Ability { get; }

        public AbilityExtension(IAbility ability)
        {
            Ability = ability;
        }
    }
}