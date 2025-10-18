using System.Collections.Generic;

namespace IM.Modules
{
    public interface IModuleExtensions
    {
        IReadOnlyList<IModuleExtension> Extensions { get; }
        
        T GetExtension<T>();
    }
}