using IM.Graphs;
using IM.Values;
using UnityEngine;

namespace IM.Modules
{
    public class SpeedExtensionsObserver : MonoBehaviour, IModuleGraphSnapshotObserver
    {
        [SerializeField] private GameObject _speedSource;
        private ModuleExtensionsObserver<ISpeedExtension> _extensionsObserver;
        private IHaveSpeed _speed;

        private void Awake()
        {
            _speed = _speedSource.GetComponent<IHaveSpeed>();

            _extensionsObserver = new ModuleExtensionsObserver<ISpeedExtension>(OnExtensionAdded, OnExtensionRemoved);
        }
        
        private void OnExtensionAdded(IExtensibleModule module,ISpeedExtension speedExt) => _speed.Speed.AddModifier(speedExt.SpeedModifier);
        private void OnExtensionRemoved(IExtensibleModule module,ISpeedExtension speedExt) => _speed.Speed.RemoveModifier(speedExt.SpeedModifier);
        public void OnGraphUpdated(IModuleGraphReadOnly graph) => _extensionsObserver.OnGraphUpdated(graph);
    }
}