using System;
using System.Linq;
using IM.Abilities;
using IM.Graphs;

namespace IM.Modules
{
    public class AbilityExtensionsObserver : IModuleGraphObserver
    {
        private readonly AbilityPool _abilityPool;
        private readonly EnumerableDiffTracker<IAbility> _diffTracker = new (); 

        public AbilityExtensionsObserver(AbilityPool abilityPool)
        {
            _abilityPool = abilityPool ?? throw new ArgumentNullException(nameof(abilityPool));
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));
            
            DiffResult<IAbility> diffResult = _diffTracker.Update(graph.Modules.Where(x => x is IGameModule module && module.Extensions.HasExtensionOfType<IAbility>())
                .SelectMany(x => (x as IGameModule).Extensions.GetExtensions<IAbility>()));
            
            foreach (IAbility ability in diffResult.Added)
            {
                _abilityPool.AddAbility(ability);
            }
            
            foreach (IAbility ability in diffResult.Removed)
            {
                _abilityPool.RemoveAbility(ability);
            }
        }
    }
}