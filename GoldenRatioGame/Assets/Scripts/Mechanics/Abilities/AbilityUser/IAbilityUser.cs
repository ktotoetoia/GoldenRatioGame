namespace IM.Abilities
{
    public interface IAbilityUser<out TAbilityPool> where TAbilityPool : IAbilityPoolReadOnly
    {
        TAbilityPool AbilityPool { get; }
        
        void UseAbility(IAbility ability, AbilityUseContext useContext);
    }
}