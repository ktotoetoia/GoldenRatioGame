using System;
using IM.WeaponSystem;

namespace IM.Modules
{
    public interface IWeaponExtension
    {
        IWeapon Weapon { get; set; }
        event Action<IWeapon> WeaponChanged;
    }
}