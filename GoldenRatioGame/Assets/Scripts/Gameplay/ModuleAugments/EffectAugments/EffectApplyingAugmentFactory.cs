using IM.Effects;
using IM.Items;
using IM.LifeCycle;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace IM.Augments
{
    [CreateAssetMenu(menuName = "Augments/Effect Augment Factory")]
    public class EffectApplyingAugmentFactory : AugmentFactory, IHaveID
    {
        [SerializeField] private AssetReference _assetReference;
        [SerializeField] private RestorableEffectGroupFactory _effectGroupFactory;
        [SerializeField] private string _name = "Augment";
        [SerializeField] private string _shortDescription= "Augment Description";
        [SerializeField] private string _desription = "Augment Description";
        [SerializeField] private Sprite _icon;
        
        public string Id => _assetReference.AssetGUID;

        public override IAugment Create(IAugmentContext ctx)
        {
            return new EffectAugment(_effectGroupFactory.Create(new EffectContext(ctx.Target,ctx.Target)))
            {
                Name = _name,
                ShortDescription = _shortDescription,
                Description = _desription,
                Icon = new Icon(_icon),
            };
        }

        public override object Save(IAugment augment)
        {
            if(augment is not IEffectAugment effectAugment) return "";

            return _effectGroupFactory.Save(effectAugment.EffectGroup);
        }

        public override IAugment Restore(object saved,IAugmentContext ctx)
        {
            return new EffectAugment(_effectGroupFactory.Restore(saved, new EffectContext(ctx.Target, ctx.Target)))     
            {
                Name = _name,
                ShortDescription = _shortDescription,
                Description = _desription,
                Icon = new Icon(_icon),
            };;
        }

    }
}