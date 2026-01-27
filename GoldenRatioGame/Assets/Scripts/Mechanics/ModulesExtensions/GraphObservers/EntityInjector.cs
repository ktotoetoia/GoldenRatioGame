using IM.Entities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class EntityInjector : MonoBehaviour, IModuleGraphSnapshotObserver
    {
        private ModuleExtensionsObserver<IRequireEntity> _extensionsObserver;
        private IEntity _entity;

        private void Awake()
        {
            _entity = GetComponent<IEntity>();

            _extensionsObserver = new ModuleExtensionsObserver<IRequireEntity>(OnExtensionAdded, OnExtensionRemoved);
        }

        private void OnExtensionAdded(IExtensibleModule module,IRequireEntity requireEntity)
        {
            if (requireEntity.Entity != null) Debug.LogWarning($"Entity already set on {requireEntity}. Overwriting with new entity.");

            requireEntity.Entity = _entity;
        }
        
        private void OnExtensionRemoved(IExtensibleModule module,IRequireEntity requireEntity)
        {
            requireEntity.Entity = null;
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph) => _extensionsObserver.OnGraphUpdated(graph);
    }
}