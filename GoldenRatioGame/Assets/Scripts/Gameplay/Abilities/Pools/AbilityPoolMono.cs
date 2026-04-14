using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Abilities
{
    public class AbilityPoolMono : MonoBehaviour, IAbilityPool, IAbilityPoolDraftContainer
    {
        private readonly IAbilityPool _abilityPool = new  AbilityPool();
        private readonly IAbilityPool _draft = new AbilityPool();
        
        public IAbilityPoolReadOnly Source => this;
        public IAbilityPoolReadOnly Draft => _draft;
        public int Count => ((IReadOnlyCollection<IAbilityReadOnly>)_abilityPool).Count;
        public bool IsReadOnly => _abilityPool.IsReadOnly;
        
        public IAbilityPool GetEditableDraft() => _draft;
        public void CommitDraft()
        {
            List<IAbilityReadOnly> toRemove = _abilityPool.Where(a => !Draft.Contains(a)).ToList();
            List<IAbilityReadOnly> toAdd = Draft.Where(a => !_abilityPool.Contains(a)).ToList();

            foreach (IAbilityReadOnly ability in toRemove) Remove(ability);
            foreach (IAbilityReadOnly ability in toAdd) Add(ability);
        }

        public IEnumerator<IAbilityReadOnly> GetEnumerator() => _abilityPool.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_abilityPool).GetEnumerator();
        public void Add(IAbilityReadOnly item) => _abilityPool.Add(item);
        public void Clear() => _abilityPool.Clear();
        public bool Contains(IAbilityReadOnly item) => _abilityPool.Contains(item);
        public void CopyTo(IAbilityReadOnly[] array, int arrayIndex) => _abilityPool.CopyTo(array, arrayIndex);
        public bool Remove(IAbilityReadOnly item) => _abilityPool.Remove(item);
    }
}