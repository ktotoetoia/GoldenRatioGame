using System.Collections;
using System.Collections.Generic;

namespace IM.Abilities
{
    public class AbilityPool : IAbilityPool
    {
        private readonly HashSet<IAbilityReadOnly> _abilities;

        public int Count => _abilities.Count;
        public bool IsReadOnly => false;

        public AbilityPool() : this(new HashSet<IAbilityReadOnly>())
        {
            
        }
        
        public AbilityPool(IEnumerable<IAbilityReadOnly> abilities)
        {
            _abilities = new HashSet<IAbilityReadOnly>(abilities);
        }

        public void Clear() => _abilities.Clear();
        public bool Contains(IAbilityReadOnly ability) => _abilities.Contains(ability);
        public void CopyTo(IAbilityReadOnly[] array, int arrayIndex) => _abilities.CopyTo(array, arrayIndex);
        public void Add(IAbilityReadOnly ability) => _abilities.Add(ability);
        public bool Remove(IAbilityReadOnly ability) => _abilities.Remove(ability);
        public IEnumerator<IAbilityReadOnly> GetEnumerator() => _abilities.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_abilities).GetEnumerator();
    }
}