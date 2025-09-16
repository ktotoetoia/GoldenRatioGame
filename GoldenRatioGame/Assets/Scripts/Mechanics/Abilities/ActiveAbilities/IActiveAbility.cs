namespace IM.Abilities
{
    public interface IActiveAbility : IAbility
    {
        bool TryUse();
    }
}