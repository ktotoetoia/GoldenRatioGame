using IM.Abilities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class AbilityExtensionsObserver : MonoBehaviour, IModuleGraphSnapshotObserver
    {
        private ModuleExtensionsObserver<IAbility> _extensionsObserver;
        private AbilityPool _abilityPool;
        
        private void Awake()
        {
            //_abilityPool = GetComponent<IModuleEntity>().AbilityPool as AbilityPool;

            _extensionsObserver = new ModuleExtensionsObserver<IAbility>(OnExtensionAdded, OnExtensionRemoved);
        }

        private void OnExtensionAdded(IExtensibleModule module,IAbility ability) => _abilityPool.AddAbility(ability);
        private void OnExtensionRemoved(IExtensibleModule module,IAbility ability) => _abilityPool.RemoveAbility(ability);
        public void OnGraphUpdated(IModuleGraphReadOnly graph) => _extensionsObserver.OnGraphUpdated(graph);
    }
}