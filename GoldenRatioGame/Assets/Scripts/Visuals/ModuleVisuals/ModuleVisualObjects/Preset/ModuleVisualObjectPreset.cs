using System;
using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    [Serializable]
    public class ModuleVisualObjectPreset : IModuleVisualObjectPreset
    {
        [SerializeField] private int _order;
        [SerializeField] private int _layer;
        [SerializeField] private bool _visible;
        [SerializeField] private LocalTransformPreset _defaultTransform = LocalTransformPreset.Default;

        [field: SerializeField] public bool PortsVisible { get; set; }
        
        public ModuleVisualObjectPreset()
        {
            
        }

        public ModuleVisualObjectPreset(int order = 0, int layer = 0, bool visible = false) : this(LocalTransformPreset.Default,order,layer,visible)
        {
            
        }
        
        public ModuleVisualObjectPreset(LocalTransformPreset localTransform,int order = 0, int layer = 0, bool visible = false)
        {
            _defaultTransform = localTransform;
            _layer = layer;
            _visible = visible;
            _order = order;
        }
        
        public void ApplyTo(IModuleVisualObject moduleVisualObject)
        {
            _defaultTransform.ApplyTo(moduleVisualObject.Transform);
            moduleVisualObject.Visible = _visible;
            moduleVisualObject.Order = _order;
            moduleVisualObject.Layer = _layer;

            foreach (IPortVisualObject portsVisualObject in moduleVisualObject.PortsVisualObjects)
            {
                portsVisualObject.Visible = PortsVisible;
            }
        }
    }
}