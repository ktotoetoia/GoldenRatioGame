using IM.Graphs;
using IM.Transforms;

namespace IM.Visuals
{
    public class PortVisualObject : IPortVisualObject
    {
        public IModuleVisualObject OwnerVisualObject { get; }
        public IPort Port { get; }
        public ITransform Transform { get; }
        
        public bool Visibility { get; set; }
        
        public PortVisualObject(IModuleVisualObject ownerVisualObject, IPort port, ITransform transform)
        {
            OwnerVisualObject = ownerVisualObject;
            Port = port;
            Transform = transform;
        }

        public void Dispose() => Visibility = false;
    }
}