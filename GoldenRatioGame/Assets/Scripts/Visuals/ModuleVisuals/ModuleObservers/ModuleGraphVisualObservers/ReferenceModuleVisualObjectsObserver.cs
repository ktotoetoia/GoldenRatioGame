using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public class ReferenceModuleVisualObjectsObserver : MonoBehaviour, IModuleGraphSnapshotObserver
    {
        private IModuleVisualMap _moduleVisualMap;
        private ModuleExtensionsObserver<IRequireReferenceModuleVisualObject> _extensionsObserver;
        
        private void Awake()
        {
            _moduleVisualMap = GetComponent<IModuleVisualMap>();
            _extensionsObserver = new ModuleExtensionsObserver<IRequireReferenceModuleVisualObject>(OnExtensionAdded, OnExtensionRemoved);
        }

        private void OnExtensionAdded(IExtensibleModule module,IRequireReferenceModuleVisualObject require) => require.SetReferenceModuleVisualObject(_moduleVisualMap.ModuleToVisualObjects[module]);
        private void OnExtensionRemoved(IExtensibleModule module,IRequireReferenceModuleVisualObject require) => require.SetReferenceModuleVisualObject(null);
        public void OnGraphUpdated(IModuleGraphReadOnly graph) => _extensionsObserver.OnGraphUpdated(graph);
    }
}