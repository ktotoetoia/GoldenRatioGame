using IM.Effects;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class EffectExtensionObserver : MonoBehaviour, IEditorObserver<IModuleGraphReadOnly>
    {
        [SerializeField] private GameObject _effectContainerSource;
        private ModuleExtensionsObserver<IEffectGroupExtension> _extensionsObserver;
        private IEffectContainer _effectContainer;
        
        private void Awake()
        {
            _effectContainer = _effectContainerSource.GetComponent<IEffectContainer>();
            
            _extensionsObserver = new ModuleExtensionsObserver<IEffectGroupExtension>(OnExtensionAdded, OnExtensionRemoved);
        }

        private void OnExtensionAdded(IExtensibleModule module, IEffectGroupExtension abilityExtension) => _effectContainer.AddGroup(abilityExtension.EffectGroup);
        private void OnExtensionRemoved(IExtensibleModule module,IEffectGroupExtension abilityExtension) => _effectContainer.RemoveGroup(abilityExtension.EffectGroup);
        public void OnSnapshotChanged(IModuleGraphReadOnly graph) => _extensionsObserver.OnSnapshotChanged(graph);
    }
}