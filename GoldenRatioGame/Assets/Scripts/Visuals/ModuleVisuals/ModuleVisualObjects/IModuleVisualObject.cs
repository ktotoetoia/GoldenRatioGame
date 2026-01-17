using System.Collections.Generic;
using IM.Graphs;
using IM.Transforms;

namespace IM.Visuals
{
    public interface IModuleVisualObject : IVisualObject
    {
        IModule Owner { get; }
        IHierarchyTransform Transform { get; }
        IReadOnlyList<IPortVisualObject> PortsVisuals { get; }
        
        IPortVisualObject GetPortVisual(IPort port);
        
        void ResetTransform();
    }
}