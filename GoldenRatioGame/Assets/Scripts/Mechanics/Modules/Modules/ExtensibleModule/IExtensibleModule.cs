using System.Collections.Generic;
using IM.Graphs;

namespace IM.Modules
{
    public interface IExtensibleModule : IModule
    {
        IReadOnlyList<IModuleExtension> Extensions { get; }
        
        T GetExtension<T>();
    }
}