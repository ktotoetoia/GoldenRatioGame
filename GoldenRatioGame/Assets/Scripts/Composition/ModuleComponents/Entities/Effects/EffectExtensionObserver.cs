using IM.Effects;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class EffectExtensionObserver : MonoBehaviour, IEditorObserver<IModuleEditingContextReadOnly>
    {
        [SerializeField] private GameObject _effectContainerSource;
        private ModuleExtensionsObserver<IEffectGroupExtension> _extensionsObserver;
        private IEffectContainer _effectContainer;
        
        private void Awake()
        {
            _effectContainer = _effectContainerSource.GetComponent<IEffectContainer>();
            
            _extensionsObserver = new ModuleExtensionsObserver<IEffectGroupExtension>(OnExtensionAdded, OnExtensionRemoved);
        }

        private void OnExtensionAdded(IExtensibleItem module, IEffectGroupExtension abilityExtension) => _effectContainer.AddGroup(abilityExtension.EffectGroup);
        private void OnExtensionRemoved(IExtensibleItem module,IEffectGroupExtension abilityExtension) => _effectContainer.RemoveGroup(abilityExtension.EffectGroup);
        public void OnSnapshotChanged(IModuleEditingContextReadOnly snapshot) => _extensionsObserver.OnSnapshotChanged(snapshot.Graph);
    }
}