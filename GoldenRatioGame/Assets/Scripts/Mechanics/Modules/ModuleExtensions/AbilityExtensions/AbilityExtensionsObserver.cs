using IM.Abilities;
using IM.Graphs;

namespace IM.Modules
{
    public class AbilityExtensionsObserver : IModuleObserver
    {
        private readonly AbilityPool _abilityPool;
        
        public AbilityExtensionsObserver(AbilityPool abilityPool)
        {
            _abilityPool = abilityPool;
        }

        public void Add(IModule module)
        {
            if (module is not IExtensibleModule componentModule || !componentModule.TryGetExtension(out IAbilityExtension extension))
            {
                return;
            }
            
            _abilityPool.AddAbility(extension.Ability);
        }

        public void Remove(IModule module)
        {
            if (module is not IExtensibleModule componentModule || !componentModule.TryGetExtension(out IAbilityExtension extension))
            {
                return;
            }
            
            _abilityPool.RemoveAbility(extension.Ability);
        }
    }
}