using System;
using System.Collections.Generic;
using IM.Abilities;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class AbilityPoolReadonlyContainer : VisualElement
    {
        private readonly Dictionary<IAbilityReadOnly, AbilityVisualElement> _abilityElements 
            = new();
        
        private IAbilityPoolDraft _abilityPoolDraft;
        
        public AbilityPoolReadonlyContainer()
        {
            AddToClassList(AbilityClassLists.AbilityContainer);
        }
        
        public void SetAbilityPool(IAbilityPoolDraft abilityPoolDraft)
        {
            if (_abilityPoolDraft != null) throw new ArgumentException("events is already set");
            
            _abilityPoolDraft = abilityPoolDraft ?? throw new ArgumentNullException(nameof(abilityPoolDraft));
            foreach (IAbilityReadOnly ability in abilityPoolDraft.Abilities)
            {
                OnAbilityAdded(ability);
            }
            
            abilityPoolDraft.AbilityAdded += OnAbilityAdded;
            abilityPoolDraft.AbilityRemoved += OnAbilityRemoved;
        }
        
        public void ClearAbilityPool()
        {
            if(_abilityPoolDraft ==  null) return;

            foreach (AbilityVisualElement element in _abilityElements.Values)
            {
                Remove(element);
            }
            
            _abilityElements.Clear();
            _abilityPoolDraft.AbilityAdded -= OnAbilityAdded;
            _abilityPoolDraft.AbilityRemoved -= OnAbilityRemoved;
            _abilityPoolDraft = null;
        }
        
        private void OnAbilityAdded(IAbilityReadOnly ability)
        {
            var ve = new AbilityVisualElement();
            ve.SetAbility(ability);
            _abilityElements[ability] = ve;
            Add(ve);
        }

        private void OnAbilityRemoved(IAbilityReadOnly ability)
        {
            if (_abilityElements.TryGetValue(ability, out var ve))
            {
                Remove(ve);
                _abilityElements.Remove(ability);
            }
        }
    }
}