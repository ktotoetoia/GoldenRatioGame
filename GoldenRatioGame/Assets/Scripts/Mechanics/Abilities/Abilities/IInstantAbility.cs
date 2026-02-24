namespace IM.Abilities
{
    public interface IInstantAbility : IAbilityReadOnly
    {
        bool TryUse();
    }
}