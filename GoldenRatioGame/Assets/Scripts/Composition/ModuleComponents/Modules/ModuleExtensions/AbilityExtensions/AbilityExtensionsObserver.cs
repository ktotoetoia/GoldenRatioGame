using IM.Abilities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class AbilityExtensionsObserver : MonoBehaviour, IEditorObserver<IModuleGraphReadOnly>
    {
        [SerializeField] private GameObject _abilityPoolSource;
        private ModuleExtensionsObserver<IAbilityExtension> _extensionsObserver;
        private IAbilityPool _abilityPool;
        
        private void Awake()
        {
            _abilityPool = _abilityPoolSource.GetComponent<IAbilityPool>();

            _extensionsObserver = new ModuleExtensionsObserver<IAbilityExtension>(OnExtensionAdded, OnExtensionRemoved);
        }

        private void OnExtensionAdded(IExtensibleModule module,IAbilityExtension abilityExtension) => _abilityPool.Add(abilityExtension.Ability);
        private void OnExtensionRemoved(IExtensibleModule module,IAbilityExtension abilityExtension) => _abilityPool.Remove(abilityExtension.Ability);
        public void OnSnapshotChanged(IModuleGraphReadOnly graph) => _extensionsObserver.OnSnapshotChanged(graph);
    }
}