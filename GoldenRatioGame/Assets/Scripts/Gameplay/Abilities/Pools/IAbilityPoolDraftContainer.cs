namespace IM.Abilities
{
    public interface IAbilityPoolDraftContainer
    {
        IAbilityPoolReadOnly Draft { get; }
        
        IAbilityPool EditDraft();
        void Commit();
    }
}