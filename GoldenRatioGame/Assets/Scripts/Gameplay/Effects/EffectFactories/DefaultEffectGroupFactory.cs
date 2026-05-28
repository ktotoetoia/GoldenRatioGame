using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Effects
{
    [CreateAssetMenu(menuName = "Effects/Default Effect Group Factory")]
    public class DefaultEffectGroupFactory : EffectGroupFactory
    {
        [SerializeField] private List<EffectModifierFactory> _effectModifierFactories;
        [SerializeField] private bool _isTemporary = true;
        [SerializeField] private float _temporaryTime = 0.5f;
        
        public override IEffectGroup Create(IEffectContext context)
        {
            return Create(new List<IEffectModifier>(),context);
        }

        public override IEffectGroup Create(IEnumerable<IEffectModifier> effectModifiers,IEffectContext context)
        {
            List<IEffectModifier> allEffectModifiers = _effectModifierFactories.Select(x => x.Create(context)).Concat(effectModifiers).ToList();
            
            return _isTemporary ? new FloatTemporaryEffectGroup(allEffectModifiers,_temporaryTime) : new EffectGroup(allEffectModifiers);
        }
    }
}