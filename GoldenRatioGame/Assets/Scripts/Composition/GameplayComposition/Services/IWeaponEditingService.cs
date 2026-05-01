using IM.Abilities;
using IM.WeaponSystem;

namespace IM.Modules
{
    public interface IWeaponEditingService : IEditingService
    {
        IContainerAbilityPoolReadOnly ContainerAbilityPoolReadOnly { get; }
        void SetWeapon(IWeaponContainerReadOnly weaponContainer, IWeapon weapon);
        void ClearWeapon(IWeaponContainerReadOnly weaponContainer);
    }
}