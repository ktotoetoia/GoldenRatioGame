using IM.Effects;

namespace IM.Augments
{
    public interface IEffectAugment : IAugment
    {
        IEffectGroup EffectGroup { get; }
    }
}