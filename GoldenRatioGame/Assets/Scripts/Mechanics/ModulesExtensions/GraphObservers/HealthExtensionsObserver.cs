using IM.Graphs;
using IM.Health;
using UnityEngine;

namespace IM.Modules
{
    public class HealthExtensionsObserver : MonoBehaviour, IModuleGraphSnapshotObserver
    {
        private ModuleExtensionsObserver<IHealthExtension> _extensionsObserver;
        private IFloatHealthValuesGroup _floatHealthValuesGroup;

        private void Awake()
        {
            _floatHealthValuesGroup = GetComponent<IFloatHealthValuesGroup>();
            
            _extensionsObserver = new ModuleExtensionsObserver<IHealthExtension>(OnExtensionAdded, OnExtensionRemoved);
        }
        
        private void OnExtensionAdded(IExtensibleModule module,IHealthExtension healthExt) => _floatHealthValuesGroup.AddHealth(healthExt.Health);
        private void OnExtensionRemoved(IExtensibleModule module,IHealthExtension healthExt) => _floatHealthValuesGroup.RemoveHealth(healthExt.Health);
        public void OnGraphUpdated(IModuleGraphReadOnly graph) => _extensionsObserver.OnGraphUpdated(graph);
    }
}