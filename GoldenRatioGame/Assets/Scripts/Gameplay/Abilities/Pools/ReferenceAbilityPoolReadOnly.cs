using System;
using System.Collections;
using System.Collections.Generic;

namespace IM.Abilities
{
    public class ReferenceAbilityPoolReadOnly : IAbilityPoolReadOnly
    {
        private readonly Func<IAbilityPoolReadOnly> _getAbilityPool;

        public ReferenceAbilityPoolReadOnly(Func<IAbilityPoolReadOnly> getAbilityPool)
        {
            _getAbilityPool = getAbilityPool;
        }

        public int Count => _getAbilityPool().Count;
        
        public IEnumerator<IAbilityReadOnly> GetEnumerator()
        {
            return _getAbilityPool().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_getAbilityPool()).GetEnumerator();
        }
        
        public bool Contains(IAbilityReadOnly item)
        {
            return _getAbilityPool().Contains(item);
        }
    }
}