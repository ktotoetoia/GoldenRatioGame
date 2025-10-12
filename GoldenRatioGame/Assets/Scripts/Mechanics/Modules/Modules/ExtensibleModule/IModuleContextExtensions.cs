using System.Collections.Generic;
using IM.Graphs;

namespace IM.Modules
{
    public interface IModuleContextExtensions
    {
        IReadOnlyList<IModuleExtension> Extensions { get; }
        
        T GetExtension<T>();
    }
}