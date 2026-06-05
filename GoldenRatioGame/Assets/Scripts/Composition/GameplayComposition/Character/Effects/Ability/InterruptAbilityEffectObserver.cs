using System.Collections.Generic;
using System.Linq;
using IM.Abilities;
using UnityEngine;

namespace IM.Effects
{
    public class InterruptAbilityEffectObserver : MonoBehaviour, IEffectObserver
    {
        [SerializeField] private GameObject _abilityUserSource;
        private IAbilityUser<IAbilityPoolReadOnly> _abilityUser;
        private readonly List<InterruptAbilityEffectModifier> _abilityInterruptingEffects = new();
        
        private void Awake()
        {
            _abilityUser = _abilityUserSource.GetComponent<IAbilityUser<IAbilityPoolReadOnly> >();
        }
        
        public void OnEffectGroupAdded(IEffectGroup group)
        {
            if (group.Modifiers.TryGetAll(out IEnumerable<InterruptAbilityEffectModifier> modifiers))
            {
                _abilityInterruptingEffects.AddRange(modifiers);
            }
            
            Evaluate();
        }

        public void OnEffectGroupRemoved(IEffectGroup group)
        {
            if (group.Modifiers.TryGetAll(out IEnumerable<InterruptAbilityEffectModifier> modifiers))
            {
                foreach(var modifier in modifiers) _abilityInterruptingEffects.Remove(modifier);
            }
            
            Evaluate();
        }

        private void Evaluate()
        {
            bool interrupted = _abilityInterruptingEffects.Any(x => x.Interrupts);
            
            _abilityUser.IsInterrupted = interrupted;
        }
    }
}