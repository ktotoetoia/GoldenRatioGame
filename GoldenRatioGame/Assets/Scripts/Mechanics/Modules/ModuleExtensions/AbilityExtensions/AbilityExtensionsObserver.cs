using System;
using System.Collections.Generic;
using System.Linq;
using IM.Abilities;
using IM.Graphs;

namespace IM.Modules
{
    public class AbilityExtensionsObserver : IModuleGraphObserver
    {
        private readonly AbilityPool _abilityPool;
        private readonly HashSet<IAbility> _knownAbilities = new();

        public AbilityExtensionsObserver(AbilityPool abilityPool)
        {
            _abilityPool = abilityPool ?? throw new ArgumentNullException(nameof(abilityPool));
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));

            HashSet<IAbility> currentAbilities = new();
        
            foreach (IModule module in graph.Modules)
            {
                if (module is IGameModule gameModule &&
                    gameModule.Extensions.TryGetExtension(out IAbilityExtension abilityExt))
                {
                    currentAbilities.Add(abilityExt.Ability);
                }
            }

            foreach (IAbility ability in currentAbilities)
            {
                if (_knownAbilities.Add(ability))
                {
                    _abilityPool.AddAbility(ability);
                }
            }

            foreach (IAbility ability in _knownAbilities.Except(currentAbilities).ToList())
            {
                _knownAbilities.Remove(ability);
                _abilityPool.RemoveAbility(ability);
            }
        }
    }
}