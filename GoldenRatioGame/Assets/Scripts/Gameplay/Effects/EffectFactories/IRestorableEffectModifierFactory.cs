using IM.LifeCycle;

namespace IM.Effects
{
    public interface IRestorableEffectModifierFactory : IEffectModifierFactory, IRestorableFactory<IEffectModifier,IEffectContext>
    {
        
    }
}