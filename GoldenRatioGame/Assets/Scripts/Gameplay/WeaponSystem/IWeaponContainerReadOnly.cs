using System;
using IM.Abilities;

namespace IM.WeaponSystem
{
    public interface IWeaponContainerReadOnly : IAbilityContainer
    {
        IWeapon DefaultWeapon { get; }
        IWeapon Weapon { get; } 
        IWeapon PreferredWeapon { get; }
        
        event Action<IWeapon> PreferredWeaponChanged; 
    }
}