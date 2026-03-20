using System.Linq;
using IM.Abilities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class AbilityPoolModuleEditingContextObserver : MonoBehaviour, IEditorObserver<IModuleGraphReadOnly>
    {
        [SerializeField] private AbilityModuleEditingContextMono _abilityModuleEditingContext;
        [SerializeField] private AbilityPoolMono _abilityPool;
        
        public void OnSnapshotChanged(IModuleGraphReadOnly snapshot)
        {
            foreach (IAbilityReadOnly abilityReadOnly in _abilityModuleEditingContext.KeyAbilityPool.Abilities.Except(_abilityPool.Abilities).ToList())
            {
                _abilityPool.AddAbility(abilityReadOnly);    
            }
            
            foreach (IAbilityReadOnly abilityReadOnly in _abilityPool.Abilities.Except(_abilityModuleEditingContext.KeyAbilityPool.Abilities).ToList())
            {
                _abilityPool.RemoveAbility(abilityReadOnly);    
            }
        }
    }
}