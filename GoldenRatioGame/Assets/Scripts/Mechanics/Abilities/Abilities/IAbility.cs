namespace IM.Abilities
{
    public interface IAbility : IAbilityReadOnly
    {
        bool TryUse();
    }
}