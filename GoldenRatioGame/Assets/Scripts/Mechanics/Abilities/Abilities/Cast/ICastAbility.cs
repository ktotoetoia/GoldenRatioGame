namespace IM.Abilities
{
    public interface ICastAbility : IAbilityReadOnly
    {
        bool TryCast(out ICastInfo info);
    }
}