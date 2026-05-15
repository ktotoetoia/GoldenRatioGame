using System;
using System.Collections.Generic;
using System.Linq;
using IM.Abilities;
using IM.WeaponSystem;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class WeaponPoolVisualContainer : VisualElement
    {
        private IContainerAbilityPoolReadOnly _containerAbilityPool;
        
        public Dictionary<IWeaponContainerReadOnly, ItemVisualElement> WeaponContainers { get; } = new();
        public event Action<IWeaponContainerReadOnly> OnContainerInteracted;
        
        public void SetContainerAbilityPool(IContainerAbilityPoolReadOnly containerAbilityPool)
        {
            ClearContainerAbilityPool();
            _containerAbilityPool = containerAbilityPool ?? throw new ArgumentNullException(nameof(containerAbilityPool));
            SyncElements();
        }

        public void ClearContainerAbilityPool()
        {
            foreach (ItemVisualElement visual in WeaponContainers.Values) Remove(visual);
            
            WeaponContainers.Clear();
            _containerAbilityPool = null;
        }

        public void Update() => SyncElements();

        private void SyncElements()
        {
            if (_containerAbilityPool == null) return;

            List<IWeaponContainerReadOnly> currentData = _containerAbilityPool.AbilityContainers.OfType<IWeaponContainerReadOnly>().ToList();

            List<IWeaponContainerReadOnly> toRemove = WeaponContainers.Keys.Except(currentData).ToList();
            foreach (IWeaponContainerReadOnly container in toRemove) RemoveWeaponContainer(container);

            List<IWeaponContainerReadOnly> toAdd = currentData.Except(WeaponContainers.Keys).ToList();
            foreach (IWeaponContainerReadOnly container in toAdd) AddWeaponContainer(container);
        }

        private void AddWeaponContainer(IWeaponContainerReadOnly weaponContainer)
        {
            ItemVisualElement itemVisualElement = new ItemVisualElement();
            weaponContainer.PreferredWeaponChanged += x => itemVisualElement.SetItem(x);
            itemVisualElement.RegisterAction("",() => OnContainerInteracted?.Invoke(weaponContainer));
            itemVisualElement.SetItem(weaponContainer.PreferredWeapon);
            WeaponContainers[weaponContainer] = itemVisualElement;
            Add(itemVisualElement);
        }

        private void RemoveWeaponContainer(IWeaponContainerReadOnly weaponContainer)
        {
            if (!WeaponContainers.TryGetValue(weaponContainer, out ItemVisualElement visual)) return;

            Remove(visual);
            visual.UnregisterActions();
            WeaponContainers.Remove(weaponContainer);
        }
    }
}