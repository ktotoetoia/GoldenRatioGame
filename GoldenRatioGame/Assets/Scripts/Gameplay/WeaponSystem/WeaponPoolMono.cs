using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IM.Abilities;
using UnityEngine;

namespace IM.WeaponSystem
{
    public class WeaponPoolMono : MonoBehaviour, IWeaponPool, IWeaponPoolDraftContainer, IAbilityPoolReadOnly
    {
        private readonly IWeaponPool _weaponPool = new WeaponPool();
        private readonly IWeaponPool _draftWeaponPool = new WeaponPool();

        private IAbilityPoolReadOnly _abilityPoolReadOnly;

        private IAbilityPoolReadOnly AbilityPoolReadOnly => _abilityPoolReadOnly ??= new WeaponToAbilityPoolWrapper(_weaponPool);

        public IWeaponPoolReadOnly Source => _weaponPool;
        public IWeaponPoolReadOnly Draft => _draftWeaponPool;

        public IWeaponPool GetEditableDraft() => _draftWeaponPool;

        public bool IsReadOnly => false;
        public int Count => _weaponPool.Count;
        public void Add(IWeapon weapon) => _weaponPool.Add(weapon);
        public bool Remove(IWeapon weapon) => _weaponPool.Remove(weapon);
        public void Clear() => _weaponPool.Clear();
        public bool Contains(IWeapon weapon) => _weaponPool.Contains(weapon);
        public void CopyTo(IWeapon[] array, int arrayIndex) => _weaponPool.CopyTo(array, arrayIndex);
        public IEnumerator<IWeapon> GetEnumerator() => _weaponPool.GetEnumerator();
        IEnumerator<IWeapon> IEnumerable<IWeapon>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void CommitDraft()
        {
            List<IWeapon> toRemove = _weaponPool.Where(w => !_draftWeaponPool.Contains(w)).ToList();
            List<IWeapon> toAdd = _draftWeaponPool.Where(w => !_weaponPool.Contains(w)).ToList();
            foreach (IWeapon weapon in toRemove) _weaponPool.Remove(weapon);
            foreach (IWeapon weapon in toAdd) _weaponPool.Add(weapon);
        }

        int IReadOnlyCollection<IAbilityReadOnly>.Count => AbilityPoolReadOnly.Count;
        public bool Contains(IAbilityReadOnly ability) => AbilityPoolReadOnly.Contains(ability);
        IEnumerator<IAbilityReadOnly> IEnumerable<IAbilityReadOnly>.GetEnumerator() => AbilityPoolReadOnly.GetEnumerator();
    }
}