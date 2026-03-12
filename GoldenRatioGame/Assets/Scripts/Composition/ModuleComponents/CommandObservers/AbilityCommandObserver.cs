using IM.Abilities;

namespace IM.Modules
{
    public class AbilityCommandObserver : ICommandObserver
    {
        private readonly IAbilityPool _abilityPool;
        private readonly IAbilityReadOnly _ability;
        
        public AbilityCommandObserver(IAbilityPool abilityPool, IAbilityReadOnly ability)
        {
            _abilityPool = abilityPool;
            _ability = ability;
        }

        public void OnModuleAdded()
        {
            _abilityPool.AddAbility(_ability);
        }

        public void OnModuleRemoved()
        {
            _abilityPool.RemoveAbility(_ability);
        }
    }
}