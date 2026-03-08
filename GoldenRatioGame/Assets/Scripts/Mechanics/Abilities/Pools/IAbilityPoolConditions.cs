namespace IM.Abilities
{
    public interface IAbilityPoolConditions
    {
        bool CanAddAbility(IAbilityReadOnly ability) => true;
        bool CanRemoveAbility(IAbilityReadOnly ability) => true;
    }
}