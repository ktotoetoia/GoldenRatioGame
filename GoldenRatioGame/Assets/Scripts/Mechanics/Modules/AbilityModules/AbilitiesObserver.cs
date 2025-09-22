using System.Collections.Generic;
using IM.Abilities;
using IM.Graphs;

namespace IM.Modules
{
    public class AbilitiesObserver : IModuleObserver,IAbilitiesPool
    {
        private readonly List<IAbility> _abilities = new();
        public IEnumerable<IAbility> Abilities => _abilities;

        public void Add(IModule module)
        {
            if (module is not IAbility ability || _abilities.Contains(ability))
            {
                return;
            }
            
            _abilities.Add(ability);
        }

        public void Remove(IModule module)
        {
            if (module is not IAbility ability || !_abilities.Contains(ability))
            {
                return;
            }
            
            _abilities.Remove(ability);
        }
    }
}