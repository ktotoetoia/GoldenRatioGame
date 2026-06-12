using IM.Effects;
using IM.Items;

namespace IM.Augments
{
    public class EffectAugment : IEffectAugment
    {
        public IEffectGroup EffectGroup { get; }
        
        public string Name { get; set; }
        public string ShortDescription { get; set;}
        public string Description { get;set; }
        public IIcon Icon { get;set; }
        
        public EffectAugment(IEffectGroup effectGroup)
        {
            EffectGroup = effectGroup;
        }
    }
}