using IM.Effects;
using UnityEngine;

namespace IM
{
    public class ApplyEffectHitContract : HitContractBase
    {
        [SerializeField] private EffectGroupFactory _effectFactory;
        
        protected override void ProcessHit(GameObject target)
        {
            if (!target.TryGetComponent(out IEffectContainer effectContainer)) return;

            IEffectContext context = new EffectContext(gameObject, target);
            
            effectContainer.AddGroup(_effectFactory.Create(context));
        }
    }
}