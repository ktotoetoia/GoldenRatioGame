using IM.Abilities;
using IM.Items;
using IM.WeaponSystem;
using UnityEngine;

namespace IM.Modules
{
    public class WeaponEditingService : IWeaponEditingService
    {
        private readonly IStorageEditingService _storageEditingService;
        public IContainerAbilityPoolReadOnly ContainerAbilityPoolReadOnly { get; }
        
        public WeaponEditingService(IContainerAbilityPoolReadOnly containerAbilityPoolReadOnly, IStorageEditingService storageEditingService)
        {
            _storageEditingService = storageEditingService;
            ContainerAbilityPoolReadOnly = containerAbilityPoolReadOnly;
        }
        
        public void SetWeapon(IWeaponContainerReadOnly weaponContainer, IWeapon weapon)
        {
            if (weaponContainer is not IWeaponContainer mutable) return;
            if(mutable.Weapon != null) ClearWeapon(weaponContainer);
            
            _storageEditingService.RemoveFromStorage(weapon as IItem);
            mutable.Weapon = weapon;
        }
        
        public void ClearWeapon(IWeaponContainerReadOnly weaponContainer)
        {
            if (weaponContainer is not IWeaponContainer { Weapon: not null } mutable) return;
            
            IWeapon weapon = mutable.Weapon;
            mutable.Weapon = null;
            _storageEditingService.AddToStorage(weapon as IItem);
        }
    }
}