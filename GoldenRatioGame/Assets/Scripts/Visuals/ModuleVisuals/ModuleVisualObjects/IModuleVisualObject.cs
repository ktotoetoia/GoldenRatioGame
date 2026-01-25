using System.Collections.Generic;
using IM.Graphs;
using IM.Modules;
using IM.Transforms;

namespace IM.Visuals
{
    public interface IModuleVisualObject : IVisualObject
    {
        IExtensibleModule Owner { get; }
        IHierarchyTransform Transform { get; }
        IReadOnlyList<IPortVisualObject> PortsVisuals { get; }
        IModuleGraphStructureUpdater ModuleGraphStructureUpdater { get; set; }
        
        IPortVisualObject GetPortVisual(IPort port);
        void Reset();
    }
}