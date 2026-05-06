using System;
using IM.Abilities;
using IM.WeaponSystem;

namespace IM.Modules
{
    public sealed class WeaponContainerWrapped : IWeaponContainer
    {
        private IWeapon _weapon;
        
        public IWeaponContainer BaseContainer { get; }

        public IWeapon DefaultWeapon { get; }
        public IWeapon Weapon
        {
            get => _weapon;
            set
            {
                if(_weapon == value) return;
                _weapon = value;
                PreferredWeaponChanged?.Invoke(PreferredWeapon);
            }
        }
        
        public IAbilityReadOnly Ability => PreferredWeapon;

        public IWeapon PreferredWeapon => Weapon ?? DefaultWeapon;

        public event Action<IWeapon> PreferredWeaponChanged; 
        
        public WeaponContainerWrapped(IWeaponContainer baseContainer)
        {
            BaseContainer = baseContainer ?? throw new ArgumentNullException(nameof(baseContainer));

            DefaultWeapon = baseContainer.DefaultWeapon;
            Weapon = baseContainer.Weapon;
        }
    }
}