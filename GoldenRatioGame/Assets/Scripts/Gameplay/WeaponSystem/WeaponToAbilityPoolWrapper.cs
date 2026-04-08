using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IM.Abilities;

namespace IM.WeaponSystem
{
    public class WeaponToAbilityPoolWrapper : IAbilityPoolReadOnly
    {
        private readonly IWeaponPoolReadOnly _weaponPoolReadOnly;

        public WeaponToAbilityPoolWrapper(IWeaponPoolReadOnly weaponPoolReadOnly)
        {
            _weaponPoolReadOnly = weaponPoolReadOnly;
        }

        public IReadOnlyCollection<IAbilityReadOnly> Abilities => _weaponPoolReadOnly.Select(x => x.Ability).ToList();
        public bool Contains(IAbilityReadOnly ability) => _weaponPoolReadOnly.Any(x => x.Ability == ability);
        public IEnumerator<IAbilityReadOnly> GetEnumerator() => Abilities.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Abilities.GetEnumerator();
        public int Count => Abilities.Count;
    }
}