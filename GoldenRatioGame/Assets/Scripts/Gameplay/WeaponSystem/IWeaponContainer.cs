using System;

namespace IM.WeaponSystem
{
    public interface IWeaponContainer
    {
        IWeapon Weapon { get; set; } 
        event Action<IWeapon> WeaponChanged; 
    }
}