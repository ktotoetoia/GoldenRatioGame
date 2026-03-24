using IM.LifeCycle;

namespace IM.Abilities
{
    public interface IAbilityPoolEditor<out TAbilityPool> : IEditor<TAbilityPool, IAbilityPoolReadOnly> where TAbilityPool : IAbilityPoolReadOnly
    {
        
    }
}