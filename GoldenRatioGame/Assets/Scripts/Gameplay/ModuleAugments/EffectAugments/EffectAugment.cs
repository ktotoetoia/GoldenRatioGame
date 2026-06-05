using IM.Effects;

namespace IM.Augments
{
    public class EffectAugment : IEffectAugment
    {
        public IEffectGroup EffectGroup { get; }
     
        public EffectAugment(IEffectGroup effectGroup)
        {
            EffectGroup = effectGroup;
        }
    }
}