using System;
using System.Collections.Generic;
using System.Linq;
using IM.Abilities;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class AbilityPoolReadonlyContainer : VisualElement
    {
        private readonly Dictionary<IAbilityReadOnly, AbilityVisualElement> _abilityElements 
            = new();
        
        private IAbilityPoolReadOnly _abilityPool;
        
        public AbilityPoolReadonlyContainer()
        {
            AddToClassList(AbilityClassLists.AbilityContainer);
        }
        
        public void SetAbilityPool(IAbilityPoolReadOnly abilityPool)
        {
            if (_abilityPool != null) throw new ArgumentException("events is already set");
            
            _abilityPool = abilityPool ?? throw new ArgumentNullException(nameof(abilityPool));
            
            foreach (IAbilityReadOnly ability in abilityPool.Abilities)
            {
                OnAbilityAdded(ability);
            }
        }
        
        public void ClearAbilityPool()
        {
            if(_abilityPool ==  null) return;

            foreach (AbilityVisualElement element in _abilityElements.Values)
            {
                Remove(element);
            }
            
            _abilityElements.Clear();
            _abilityPool = null;
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

        public void Update()
        {
            if(_abilityPool == null) return;
            
            foreach (IAbilityReadOnly ability in _abilityPool.Abilities.Except(_abilityElements.Keys).ToList())
            {
                OnAbilityAdded(ability);
            }
            
            foreach (IAbilityReadOnly ability in _abilityElements.Keys.Except(_abilityPool.Abilities).ToList())
            {
                OnAbilityRemoved(ability);
            }
        }
    }
}