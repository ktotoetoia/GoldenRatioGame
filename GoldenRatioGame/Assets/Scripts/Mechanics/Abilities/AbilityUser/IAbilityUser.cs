using System;

namespace IM.Abilities
{
    public interface IAbilityUser<out TAbilityPool> where TAbilityPool : IAbilityPoolReadOnly
    {
        TAbilityPool AbilityPool { get; }
        Func<IAbilityReadOnly, AbilityUseContext>  GetAbilityUseContext { get; set; }
        
        bool CanUseAbility(IAbilityReadOnly ability);
        void UseAbility(IAbilityReadOnly ability);
        
    }
}