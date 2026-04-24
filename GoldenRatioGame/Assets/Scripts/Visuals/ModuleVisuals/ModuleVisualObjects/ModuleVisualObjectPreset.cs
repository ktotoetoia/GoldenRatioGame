using System;
using UnityEngine;

namespace IM.Visuals
{
    [Serializable]
    public class ModuleVisualObjectPreset : IVisualObjectPreset
    {
        [SerializeField] private VisualObjectPreset _preset;
        private readonly IVisualObjectPreset _providedPreset;
        
        [field: SerializeField] public bool PortsVisible { get; set; }
        private IVisualObjectPreset Preset => _providedPreset ?? _preset;

        public ModuleVisualObjectPreset()
        {
            
        }
        
        public ModuleVisualObjectPreset(int order = 0, int layer = 0, bool visible = false) : this(new VisualObjectPreset(order,layer,visible))
        {
            
        }
        
        public ModuleVisualObjectPreset(IVisualObjectPreset preset)
        {
            _providedPreset = preset;
        }
        
        public void ApplyTo(IVisualObject visualObject)
        {
            Preset?.ApplyTo(visualObject);

            if (visualObject is not IModuleVisualObject moduleVisualObject) return;
            
            foreach (IPortVisualObject portsVisualObject in moduleVisualObject.PortsVisualObjects) 
                portsVisualObject.Visible = PortsVisible;
        }
    }
}