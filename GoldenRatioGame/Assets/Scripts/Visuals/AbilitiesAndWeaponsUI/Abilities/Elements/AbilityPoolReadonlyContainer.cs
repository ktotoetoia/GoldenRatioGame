using System.Collections.Generic;
using System.Linq;
using IM.Abilities;
using IM.WeaponSystem;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class AbilityPoolReadonlyContainer : VisualElement
    {
        private readonly Dictionary<IAbilityReadOnly, ItemVisualElement> _abilityElements = new();
        private IAbilityPoolReadOnly _abilityPool;
        private bool _viewWeaponsAsAbilities;

        public AbilityPoolReadonlyContainer()
        {
            AddToClassList(AbilityClassLists.AbilityContainer);
        }

        public bool ViewWeaponsAsAbilities
        {
            get => _viewWeaponsAsAbilities;
            set
            {
                _viewWeaponsAsAbilities = value;
                SetAbilityPool(_abilityPool);
            }
        }

        public void SetAbilityPool(IAbilityPoolReadOnly abilityPool)
        {
            ClearAbilityPool();
            _abilityPool = abilityPool;
            
            if(abilityPool == null) return;
            
            SyncElements();
        }

        public void ClearAbilityPool()
        {
            foreach (ItemVisualElement element in _abilityElements.Values) Remove(element);

            _abilityElements.Clear();
            _abilityPool = null;
        }

        public void Update() => SyncElements();

        private void SyncElements()
        {
            if (_abilityPool == null) return;
            
            List<IAbilityReadOnly> toRemove = _abilityElements.Keys.Except(_abilityPool).ToList();
            List<IAbilityReadOnly> toAdd = _abilityPool.Except(_abilityElements.Keys).ToList();
            
            if (!ViewWeaponsAsAbilities)
            {
                toAdd.RemoveAll(x => x is IWeapon);
                toRemove.AddRange(_abilityElements.Keys.Where(x => x is IWeapon));
            }
            
            foreach (IAbilityReadOnly ability in toRemove) RemoveAbility(ability);
            foreach (IAbilityReadOnly ability in toAdd) AddAbility(ability);
        }

        private void AddAbility(IAbilityReadOnly ability)
        {
            ItemVisualElement visualElement = new ItemVisualElement();
            visualElement.SetItem(ability);
            
            _abilityElements[ability] = visualElement;
            Add(visualElement);
        }

        private void RemoveAbility(IAbilityReadOnly ability)
        {
            if (!_abilityElements.TryGetValue(ability, out ItemVisualElement element)) return;
            
            Remove(element);
            _abilityElements.Remove(ability);
        }
    }
}