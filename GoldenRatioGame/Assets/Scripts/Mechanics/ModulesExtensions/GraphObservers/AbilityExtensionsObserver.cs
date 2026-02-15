using IM.Abilities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class AbilityExtensionsObserver : MonoBehaviour, IModuleGraphSnapshotObserver
    {
        private ModuleExtensionsObserver<IAbilityExtension> _extensionsObserver;
        private IAbilityPool _abilityPool;
        
        private void Awake()
        {
            _abilityPool = GetComponent<IAbilityPool>();

            _extensionsObserver = new ModuleExtensionsObserver<IAbilityExtension>(OnExtensionAdded, OnExtensionRemoved);
        }

        private void OnExtensionAdded(IExtensibleModule module,IAbilityExtension abilityExtension) => _abilityPool.AddAbility(abilityExtension.Ability);
        private void OnExtensionRemoved(IExtensibleModule module,IAbilityExtension abilityExtension) => _abilityPool.RemoveAbility(abilityExtension.Ability);
        public void OnGraphUpdated(IModuleGraphReadOnly graph) => _extensionsObserver.OnGraphUpdated(graph);
    }
}