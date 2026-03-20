using System.Collections.Generic;
using IM.Modules;

namespace IM.Visuals
{
    public interface IModuleVisualMap
    {
        IReadOnlyDictionary<IExtensibleModule, IModuleVisualObject> ModuleToVisualObjects { get; }
    }
}