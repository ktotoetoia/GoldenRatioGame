using System.Collections.Generic;
using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public class EntityModuleGraphVisualObserver : MonoBehaviour, IModuleVisualMap, IEditorObserver<IModuleEditingContextReadOnly>
    {
        [SerializeField] private ModuleVisualObjectPreset _preset;
        private ModuleGraphVisualObserver _moduleGraphVisualObserver;
        public IReadOnlyDictionary<IDataModule<IExtensibleItem>, IModuleVisualObject> ModuleToVisualObjects => _moduleGraphVisualObserver.ModuleToVisualObjects;

        public IReadOnlyDictionary<IDataPort<IExtensibleItem>, IPortVisualObject> PortToVisualObjects =>
            _moduleGraphVisualObserver.PortToVisualObjects;

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
        
        public void OnSnapshotChanged(IModuleEditingContextReadOnly snapshot) 
        {
            _moduleGraphVisualObserver.OnSnapshotChanged(snapshot);
            
            if(!isActiveAndEnabled) return;
            _moduleGraphVisualObserver.Update();    
        }

        private void OnEnable()
        {
            _moduleGraphVisualObserver.Update();
        }
    }
}