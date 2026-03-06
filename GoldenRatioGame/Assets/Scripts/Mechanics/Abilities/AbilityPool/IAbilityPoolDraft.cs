using IM.Abilities;

namespace IM.UI
{
    public interface IAbilityPoolDraft : IAbilityPool, IAbilityPoolEvents
    {
        void Commit();
        void Rollback();
    }
}