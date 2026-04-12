using IM.Graphs;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Modules
{
    public class EntityInjector : MonoBehaviour, IEditorObserver<IModuleEditingContextReadOnly>
    {
        [SerializeField] private GameObject _entitySource;
        private ModuleExtensionsObserver<IRequireEntity> _extensionsObserver;
        private IEntity _entity;

        private void Awake()
        {
            _entity = _entitySource.GetComponent<IEntity>();

            _extensionsObserver = new ModuleExtensionsObserver<IRequireEntity>(OnExtensionAdded, OnExtensionRemoved);
        }

        private void OnExtensionAdded(IExtensibleItem module,IRequireEntity requireEntity)
        {
            if (requireEntity.Entity != null) Debug.LogWarning($"Entity already set on {requireEntity}. Overwriting with new entity.");

            requireEntity.Entity = _entity;
        }
        
        private void OnExtensionRemoved(IExtensibleItem module,IRequireEntity requireEntity)
        {
            requireEntity.Entity = null;
        }

        public void OnSnapshotChanged(IModuleEditingContextReadOnly snapshot) => _extensionsObserver.OnSnapshotChanged(snapshot.Graph);
    }
}