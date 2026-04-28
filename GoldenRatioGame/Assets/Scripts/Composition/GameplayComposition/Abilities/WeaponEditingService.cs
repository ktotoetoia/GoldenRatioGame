using System.Linq;
using IM.Abilities;
using IM.Storages;
using IM.WeaponSystem;

namespace IM.Modules
{
    public class WeaponEditingService : IEditingService
    {
        private readonly ICellFactoryStorage _storage;
        public IContainerAbilityPoolReadOnly ContainerAbilityPoolReadOnly { get; }
        
        public WeaponEditingService(IContainerAbilityPoolReadOnly containerAbilityPoolReadOnly, ICellFactoryStorage storage)
        {
            _storage = storage;
            ContainerAbilityPoolReadOnly = containerAbilityPoolReadOnly;
        }
        
        public void SetWeapon(IWeaponContainerReadOnly weaponContainer, IWeapon weapon)
        {
            ((IWeaponContainer)weaponContainer).Weapon = weapon;
            _storage.ClearCell(_storage.FirstOrDefault(x => x.Item?.Equals(weapon) ?? false));
        }
    }
}