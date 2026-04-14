using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleGraphVisualObserver : ModuleGraphVisualObserverBase
    {
        private readonly ModuleGraphSnapshotDiffer _snapshotDiffer;
        private readonly Transform _parent;
        private readonly IModuleVisualObjectPreset _preset;
        
        public bool ShowPortsOnConnected { get; set; } = false;
        public bool ShowPortsOnDisconnected { get; set; } = true;
        
        public ModuleGraphVisualObserver(Transform parent, bool useInGameObjectPool) : this(parent,useInGameObjectPool,new ModuleVisualObjectPreset(visible:true))
        {
            
        }
        
        public ModuleGraphVisualObserver(Transform parent, bool useInGameObjectPool, IModuleVisualObjectPreset preset) : base(useInGameObjectPool)
        {
            _parent = parent;
            _preset = preset;
        }
        
        protected override void HandleModuleAdded(IDataModule<IExtensibleItem> extensibleModule,IModuleVisualObjectProvider moduleVisualObjectProvider)
        {
            IModuleVisualObject visualObject = GetObjectPool(moduleVisualObjectProvider).Get();
            ModuleVisualObjects.Add(extensibleModule, visualObject);
            visualObject.Transform.Transform.SetParent(_parent, false);

            int i = 0;
            
            foreach (IDataPort<IExtensibleItem> port in extensibleModule.DataPorts)
            {
                PortsVisualObjects.Add(port, visualObject.PortsVisualObjects[i]);
                i++;
            }
            
            _preset.ApplyTo(visualObject);
        }
        
        protected override void HandleModuleRemoved(IDataModule<IExtensibleItem>  extensibleModule)
        {
            ModuleVisualObjects.Remove(extensibleModule, out IModuleVisualObject visualObject);
            
            foreach (IDataPort<IExtensibleItem> port in extensibleModule.DataPorts)
            {
                PortsVisualObjects.Remove(port);
            }
            
            GetObjectPool(extensibleModule.Value.Extensions.Get<IModuleVisualObjectProvider>()).Release(visualObject);
        }

        protected override void HandleConnected(IPortVisualObject portA, IPortVisualObject portB)
        {
            (portA,portB) = ResolvePorts(portA, portB);
            
            PortAligner.AlignPorts(portA,portB);
            portA.Visible = ShowPortsOnConnected;
            portB.Visible = ShowPortsOnConnected;
        }

        private (IPortVisualObject, IPortVisualObject) ResolvePorts(IPortVisualObject portA, IPortVisualObject portB)
        {
            IExtensibleItem moduleA = portA.OwnerVisualObject.Owner;
            IExtensibleItem moduleB = portB.OwnerVisualObject.Owner;

            foreach ((IDataModule<IExtensibleItem>  module, IModuleVisualObject obj) in ModuleVisualObjects)
            {
                if (module.Value == moduleA) return (portB, portA);
                if (module.Value == moduleB) return (portA, portB);
            }
            
            return (portA, portB);
        }

        protected override  void HandleDisconnected(IPortVisualObject portA, IPortVisualObject portB)
        {
            portA.Visible = ShowPortsOnDisconnected;
            portB.Visible = ShowPortsOnDisconnected;
        }
    }
}