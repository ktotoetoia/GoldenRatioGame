using System;
using System.Collections.Generic;
using System.Linq;
using IM.Abilities;
using IM.WeaponSystem;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class WeaponVisualContainer : VisualElement
    {
        private IContainerAbilityPool _containerAbilityPool;
        
        public Dictionary<IWeaponContainer, ItemVisualElement> WeaponContainers { get; } = new();

        public void SetContainerAbilityPool(IContainerAbilityPool containerAbilityPool)
        {
            ClearContainerAbilityPool();
            _containerAbilityPool = containerAbilityPool ?? throw new ArgumentNullException(nameof(containerAbilityPool));
            SyncElements();
        }

        public void ClearContainerAbilityPool()
        {
            foreach (VisualElement visual in WeaponContainers.Values) Remove(visual);
            
            WeaponContainers.Clear();
            _containerAbilityPool = null;
        }

        public void Update() => SyncElements();

        private void SyncElements()
        {
            if (_containerAbilityPool == null) return;

            List<IWeaponContainer> currentData = _containerAbilityPool.AbilityContainers.OfType<IWeaponContainer>().ToList();

            List<IWeaponContainer> toRemove = WeaponContainers.Keys.Except(currentData).ToList();
            foreach (IWeaponContainer container in toRemove) RemoveWeaponContainer(container);

            List<IWeaponContainer> toAdd = currentData.Except(WeaponContainers.Keys).ToList();
            foreach (IWeaponContainer container in toAdd) AddWeaponContainer(container);
        }

        private void AddWeaponContainer(IWeaponContainer weaponContainer)
        {
            ItemVisualElement itemVisualElement = new ItemVisualElement();
            weaponContainer.PreferredWeaponChanged += x => itemVisualElement.SetItem(x);
            itemVisualElement.SetItem(weaponContainer.PreferredWeapon);
            WeaponContainers[weaponContainer] = itemVisualElement;
            Add(itemVisualElement);
        }

        private void RemoveWeaponContainer(IWeaponContainer weaponContainer)
        {
            if (!WeaponContainers.TryGetValue(weaponContainer, out ItemVisualElement visual)) return;

            Remove(visual);
            WeaponContainers.Remove(weaponContainer);
        }
    }
}