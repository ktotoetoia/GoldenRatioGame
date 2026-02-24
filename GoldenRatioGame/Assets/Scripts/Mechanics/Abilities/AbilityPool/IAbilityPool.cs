namespace IM.Abilities
{
    public interface IAbilityPool : IAbilityPoolReadOnly
    {
        void AddAbility(IAbilityReadOnly ability);
        void RemoveAbility(IAbilityReadOnly ability);
    }
}