using IM.Abilities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class AbilityExtensionsObserver : MonoBehaviour, IEditorObserver<IModuleEditingContextReadOnly>
    {
        [SerializeField] private GameObject _abilityPoolSource;
        private ModuleExtensionsObserver<IAbilityExtension> _extensionsObserver;
        private IAbilityPool _abilityPool;
        
        private void Awake()
        {
            _abilityPool = _abilityPoolSource.GetComponent<IAbilityPool>();

            _extensionsObserver = new ModuleExtensionsObserver<IAbilityExtension>(OnExtensionAdded, OnExtensionRemoved);
        }

        private void OnExtensionAdded(IExtensibleItem module,IAbilityExtension abilityExtension) => _abilityPool.Add(abilityExtension.Ability);
        private void OnExtensionRemoved(IExtensibleItem module,IAbilityExtension abilityExtension) => _abilityPool.Remove(abilityExtension.Ability);
        public void OnSnapshotChanged(IModuleEditingContextReadOnly snapshot) => _extensionsObserver.OnSnapshotChanged(snapshot.Graph);
    }
}