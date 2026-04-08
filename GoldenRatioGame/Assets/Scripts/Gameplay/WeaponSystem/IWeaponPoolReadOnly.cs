using System.Collections.Generic;

namespace IM.WeaponSystem
{
    public interface IWeaponPoolReadOnly : IReadOnlyCollection<IWeapon>
    {
        bool Contains(IWeapon weapon);
    }
}