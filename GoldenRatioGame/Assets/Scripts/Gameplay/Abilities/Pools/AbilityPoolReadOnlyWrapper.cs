using System.Collections.Generic;

namespace IM.Abilities
{
    public class AbilityPoolReadOnlyWrapper : IAbilityPoolReadOnly
    {
        private readonly IAbilityPoolReadOnly _source;
        
        public IReadOnlyCollection<IAbilityReadOnly> Abilities => _source.Abilities;

        public AbilityPoolReadOnlyWrapper(IAbilityPoolReadOnly source)
        {
            _source = source;
        }
        
        public bool Contains(IAbilityReadOnly ability)
        {
            return _source.Contains(ability);
        }
    }
}