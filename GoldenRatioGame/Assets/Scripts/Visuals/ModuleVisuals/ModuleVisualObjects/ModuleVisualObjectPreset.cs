using System;
using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    [Serializable]
    public class ModuleVisualObjectPreset
    {
        [SerializeField] private int _order;
        [SerializeField] private int _layer;
        [SerializeField] private bool _visible;
        private LocalTransformReadOnly _defaultTransform;

        public ModuleVisualObjectPreset()
        {
            _defaultTransform = LocalTransformReadOnly.Default;
        }

        public ModuleVisualObjectPreset(int order = 0, int layer = 0, bool visible = false) : this(LocalTransformReadOnly.Default,order,layer,visible)
        {
            
        }
        
        public ModuleVisualObjectPreset(LocalTransformReadOnly localTransform,int order = 0, int layer = 0, bool visible = false)
        {
            _defaultTransform = localTransform;
            _layer = layer;
            _visible = visible;
            _order = order;
        }
        public void ApplyTo(IModuleVisualObject moduleVisualObject)
        {
            moduleVisualObject.Visible = _visible;
            _defaultTransform.ApplyTo(moduleVisualObject.Transform);
            moduleVisualObject.Order = _order;
            moduleVisualObject.Transform.Transform.gameObject.layer = _layer;
        }
    }
}