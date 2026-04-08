using System.Collections;
using System.Collections.Generic;

namespace IM.WeaponSystem
{
    public class WeaponPool : IWeaponPool
    {
        private HashSet<IWeapon> _weapons;
        
        public int Count => _weapons.Count;
        public bool IsReadOnly => false;
        public IEnumerator<IWeapon> GetEnumerator() => _weapons.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_weapons).GetEnumerator();
        public void Add(IWeapon item) => _weapons.Add(item);
        public void Clear() => _weapons.Clear();
        public bool Contains(IWeapon item) => _weapons.Contains(item);
        public void CopyTo(IWeapon[] array, int arrayIndex) => _weapons.CopyTo(array, arrayIndex);
        public bool Remove(IWeapon item) => _weapons.Remove(item);
    }
}