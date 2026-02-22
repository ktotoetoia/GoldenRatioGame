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
        private Palette _corePalette;
        
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
        
        protected override void HandleModuleAdded(IExtensibleModule extensibleModule,IModuleVisual moduleVisual)
        {
            IModuleVisualObject visualObject = GetObjectPool(moduleVisual).Get();
            
            ModuleVisualObjects.Add(extensibleModule, visualObject);
            visualObject.Transform.Transform.SetParent(_parent, false);
            _preset.ApplyTo(visualObject);
            
            if (extensibleModule is ICoreExtensibleModule)
            {
                _corePalette =visualObject.PaletteSwapper.SourcePalette;

                foreach (var moduleVisualObject in ModuleVisualObjects.Values) ChangePalette(moduleVisualObject);
                
                return;
            }

            ChangePalette(visualObject);
        }

        private void ChangePalette(IModuleVisualObject visualObject)
        {
            visualObject.PaletteSwapper.AppliedPalette =  _corePalette;
        }

        protected override  void HandleModuleRemoved(IExtensibleModule extensibleModule)
        {
            ModuleVisualObjects.Remove(extensibleModule, out IModuleVisualObject visualObject);
            GetObjectPool(extensibleModule.Extensions.GetExtension<IModuleVisual>()).Release(visualObject);
        }

        protected override void HandleConnected(IPortVisualObject portA, IPortVisualObject portB)
        {
            PortAligner.AlignPorts(portA,portB);
            portA.Visible = ShowPortsOnConnected;
            portB.Visible = ShowPortsOnConnected;
        }

        protected override  void HandleDisconnected(IPortVisualObject portA, IPortVisualObject portB)
        {
            portA.Visible = ShowPortsOnDisconnected;
            portB.Visible = ShowPortsOnDisconnected;
        }
    }
}