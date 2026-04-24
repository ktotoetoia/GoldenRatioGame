using System;
using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    public interface IVisualObjectPreset
    {
        void ApplyTo(IVisualObject visualObject);
    }
    
    [Serializable]
    public class VisualObjectPreset : IVisualObjectPreset
    {
        [field: SerializeField] public int Order { get; private set; }
        [field: SerializeField] public int Layer{ get; private set; }
        [field: SerializeField] public bool Visible{ get; private set; }
        [field: SerializeField] public LocalTransformPreset DefaultTransform { get; private set; } = LocalTransformPreset.Default;

        public VisualObjectPreset()
        {
            
        }

        public VisualObjectPreset(int order = 0, int layer = 0, bool visible = false) : this(LocalTransformPreset.Default,order,layer,visible)
        {
            
        }
        
        public VisualObjectPreset(LocalTransformPreset localTransform,int order = 0, int layer = 0, bool visible = false)
        {
            DefaultTransform = localTransform;
            Layer = layer;
            Visible = visible;
            Order = order;
        }
        
        public virtual void ApplyTo(IVisualObject visualObject)
        {
            DefaultTransform.ApplyTo(visualObject.Transform);
            visualObject.Visible = Visible;
            visualObject.Order = Order;
            visualObject.Layer = Layer;
        }
    }
}