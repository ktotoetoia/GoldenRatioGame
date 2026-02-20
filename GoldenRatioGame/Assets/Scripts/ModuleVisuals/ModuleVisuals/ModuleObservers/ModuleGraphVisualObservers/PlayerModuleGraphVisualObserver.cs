using System.Collections.Generic;
using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public class PlayerModuleGraphVisualObserver : MonoBehaviour, IModuleGraphSnapshotObserver, IModuleVisualMap
    {
        [SerializeField] private ModuleVisualObjectPreset _preset;
        private ModuleGraphVisualObserver _moduleGraphVisualObserver;
        public IReadOnlyDictionary<IExtensibleModule, IModuleVisualObject> ModuleToVisualObjects => _moduleGraphVisualObserver.ModuleToVisualObjects;

        private void Awake()
        {
            _moduleGraphVisualObserver = new ModuleGraphVisualObserver(transform, true, _preset)
            {
                ShowPortsOnDisconnected = false,
                ShowPortsOnConnected = false
            };
        }

        private void Update()
        {
            _moduleGraphVisualObserver.Update();    
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph) 
        {
            _moduleGraphVisualObserver.OnGraphUpdated(graph);
            _moduleGraphVisualObserver.Update();    
        }
    }
}