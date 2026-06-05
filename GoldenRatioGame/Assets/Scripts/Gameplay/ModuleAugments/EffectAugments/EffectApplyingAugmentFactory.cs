using IM.Effects;
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
        public string Id => _assetReference.AssetGUID;

        public override IAugment Create(IAugmentContext ctx)
        {
            return new EffectAugment(_effectGroupFactory.Create(new EffectContext(ctx.Target,ctx.Target)));
        }

        public override object Save(IAugment augment)
        {
            if(augment is not IEffectAugment effectAugment) return "";

            return _effectGroupFactory.Save(effectAugment.EffectGroup);
        }

        public override IAugment Restore(object saved,IAugmentContext ctx)
        {
            return new EffectAugment(_effectGroupFactory.Restore(saved, new EffectContext(ctx.Target, ctx.Target)));
        }

    }
}