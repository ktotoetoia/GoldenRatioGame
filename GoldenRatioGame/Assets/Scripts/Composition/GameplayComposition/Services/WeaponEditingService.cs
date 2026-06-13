using IM.Abilities;
using IM.Graphs;
using IM.Items;
using IM.WeaponSystem;

namespace IM.Modules
{
    public class WeaponEditingService : IWeaponEditingService
    {
        private readonly IStorageEditingService _storageEditingService;
        private readonly AbilityPoolEditingService _abilityPoolEditingService;
        public IContainerAbilityPoolReadOnly ContainerAbilityPoolReadOnly { get; }
        
        public WeaponEditingService(
            IContainerAbilityPoolReadOnly containerAbilityPoolReadOnly, 
            IStorageEditingService storageEditingService,
            IGraphEditingEvents<IExtensibleItem> graphEditingEvents, AbilityPoolEditingService abilityPoolEditingService)
        {
            _storageEditingService = storageEditingService;
            _abilityPoolEditingService = abilityPoolEditingService;
            ContainerAbilityPoolReadOnly = containerAbilityPoolReadOnly;
            
            graphEditingEvents.Removed += OnModuleRemoved;
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

        private void OnModuleRemoved(IDataModule<IExtensibleItem> module)
        {
            if (module?.Value == null) return;
            
            if (module.Value.Extensions.TryGet(out IWeaponContainerReadOnly weaponContainer))
            {
                ClearWeapon(_abilityPoolEditingService.GetWrapped(weaponContainer) as IWeaponContainerReadOnly);
            }
        }
    }
}