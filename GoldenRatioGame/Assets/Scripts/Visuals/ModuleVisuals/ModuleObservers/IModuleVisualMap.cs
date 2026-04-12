using System.Collections.Generic;
using IM.Graphs;
using IM.Modules;

namespace IM.Visuals
{
    public interface IModuleVisualMap
    {
        IReadOnlyDictionary<IDataModule<IExtensibleItem>, IModuleVisualObject> ModuleToVisualObjects { get; }
        IReadOnlyDictionary<IDataPort<IExtensibleItem>, IPortVisualObject> PortToVisualObjects { get; }
    }
}