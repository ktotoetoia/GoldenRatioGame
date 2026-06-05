using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Effects
{
    [CreateAssetMenu(menuName = "Effects/Default Restorable Effect Group Factory")]
    public class DefaultRestorableEffectGroupFactory : RestorableEffectGroupFactory
    {
        [SerializeReference] [SerializeReferenceDropdown] private List<IRestorableEffectModifierFactory> _effectModifierFactories;
        
        public override IEffectGroup Create(IEffectContext context)
        {
            return Create(new List<IEffectModifier>(),context);
        }

        public override IEffectGroup Create(IEnumerable<IEffectModifier> effectModifiers,IEffectContext context)
        {
            List<IEffectModifier> allEffectModifiers = _effectModifierFactories.Select(x => x.Create(context)).Concat(effectModifiers).ToList();
            
            return new EffectGroup(allEffectModifiers);
        }

        public override object Save(IEffectGroup group)
        {
            SavedEffectGroup savedEffect = new();
            int i = 0;
            
            foreach (IEffectModifier modifier in group.Modifiers.Collection)
            {
                savedEffect.EffectModifierObjects.Add(_effectModifierFactories[i].Save(modifier));
                
                i++;
            }

            return savedEffect;
        }

        public override IEffectGroup Restore(object modifier, IEffectContext ctx)
        { 
            if (modifier is not SavedEffectGroup mod) return null;
            List<IEffectModifier> result = new();
            int i = 0;
            
            foreach (object obj in mod.EffectModifierObjects)
            {
                result.Add(_effectModifierFactories[i].Restore(obj,ctx));
                
                i++;
            }
            
            return new EffectGroup(result);
        }
        
        [Serializable]
        private class SavedEffectGroup
        {
            public List<object> EffectModifierObjects = new();
        }
    }
}