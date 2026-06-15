using IM.LifeCycle;

namespace IM.Effects
{
    public interface IRestorableEffectGroupFactory : IEffectGroupFactory, IRestorableFactory<IEffectGroup, IEffectContext>
    {
        
    }
}