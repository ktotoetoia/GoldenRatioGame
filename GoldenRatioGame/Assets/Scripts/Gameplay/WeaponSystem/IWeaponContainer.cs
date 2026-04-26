using System;
using IM.Abilities;

namespace IM.WeaponSystem
{
    public interface IWeaponContainer : IAbilityContainer
    {
        IWeapon DefaultWeapon { get; }
        IWeapon Weapon { get; set; } 
        IWeapon PreferredWeapon { get; }
        
        event Action<IWeapon> PreferredWeaponChanged; 
    }
}