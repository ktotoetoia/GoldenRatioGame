using IM.Graphs;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Modules
{
    public class EntityInjector : MonoBehaviour, IEditorObserver<IModuleEditingContextReadOnly>
    {
        [SerializeField] private GameObject _entitySource;
        private ModuleExtensionsObserver<IRequireEntityExtension> _extensionsObserver;
        private IEntity _entity;

        private void Awake()
        {
            _entity = _entitySource.GetComponent<IEntity>();

            _extensionsObserver = new ModuleExtensionsObserver<IRequireEntityExtension>(OnExtensionAdded, OnExtensionRemoved);
        }

        private void OnExtensionAdded(IExtensibleItem module,IRequireEntityExtension requireEntityExtension)
        {
            if (requireEntityExtension.Entity != null) Debug.LogWarning($"Entity already set on {requireEntityExtension}. Overwriting with new entity.");

            requireEntityExtension.Entity = _entity;
        }
        
        private void OnExtensionRemoved(IExtensibleItem module,IRequireEntityExtension requireEntityExtension)
        {
            requireEntityExtension.Entity = null;
        }

        public void OnSnapshotChanged(IModuleEditingContextReadOnly snapshot) => _extensionsObserver.OnSnapshotChanged(snapshot.Graph);
    }
}