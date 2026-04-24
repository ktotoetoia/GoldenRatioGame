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
        private readonly Dictionary<IWeaponContainer, VisualElement> _weaponContainers = new();
        private IContainerAbilityPool _containerAbilityPool;

        public void SetContainerAbilityPool(IContainerAbilityPool containerAbilityPool)
        {
            ClearContainerAbilityPool();
            _containerAbilityPool = containerAbilityPool ?? throw new ArgumentNullException(nameof(containerAbilityPool));
            SyncElements();
        }

        public void ClearContainerAbilityPool()
        {
            foreach (VisualElement visual in _weaponContainers.Values)
                Remove(visual);

            _weaponContainers.Clear();
            _containerAbilityPool = null;
        }

        public void Update() => SyncElements();

        private void SyncElements()
        {
            if (_containerAbilityPool == null) return;

            List<IWeaponContainer> currentData = _containerAbilityPool.AbilityContainers.OfType<IWeaponContainer>().ToList();

            List<IWeaponContainer> toRemove = _weaponContainers.Keys.Except(currentData).ToList();
            foreach (IWeaponContainer container in toRemove) RemoveWeaponContainer(container);

            List<IWeaponContainer> toAdd = currentData.Except(_weaponContainers.Keys).ToList();
            foreach (IWeaponContainer container in toAdd) AddWeaponContainer(container);
        }

        private void AddWeaponContainer(IWeaponContainer weaponContainer)
        {
            ItemVisualElement itemVisualElement = new ItemVisualElement();
            itemVisualElement.SetItem(weaponContainer.Weapon);
            _weaponContainers[weaponContainer] = itemVisualElement;
            Add(itemVisualElement);
        }

        private void RemoveWeaponContainer(IWeaponContainer weaponContainer)
        {
            if (!_weaponContainers.TryGetValue(weaponContainer, out VisualElement visual)) return;

            Remove(visual);
            _weaponContainers.Remove(weaponContainer);
        }
    }
}