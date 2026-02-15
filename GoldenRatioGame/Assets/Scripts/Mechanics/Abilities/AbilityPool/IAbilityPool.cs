namespace IM.Abilities
{
    public interface IAbilityPool : IAbilityPoolReadOnly
    {
        void AddAbility(IAbility ability);
        void RemoveAbility(IAbility ability);
    }
}