using System;
using System.Collections;
using System.Collections.Generic;

namespace IM.Abilities
{
    public class ReferenceAbilityPoolReadOnly : IAbilityPoolReadOnly
    {
        private readonly Func<IAbilityPoolReadOnly> _getAbilityPool;

        public int Count => _getAbilityPool().Count;
        
        public ReferenceAbilityPoolReadOnly(Func<IAbilityPoolReadOnly> getAbilityPool)
        {
            _getAbilityPool = getAbilityPool;
        }

        public IEnumerator<IAbilityReadOnly> GetEnumerator() => _getAbilityPool().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_getAbilityPool()).GetEnumerator();
        public bool Contains(IAbilityReadOnly item) => _getAbilityPool().Contains(item);
    }
}