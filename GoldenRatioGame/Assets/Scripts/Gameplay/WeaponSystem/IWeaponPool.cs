using System.Collections.Generic;

namespace IM.WeaponSystem
{
    public interface IWeaponPool : IWeaponPoolReadOnly, ICollection<IWeapon>
    {
        new int Count { get; }
        new bool Contains(IWeapon weapon);
    }
}