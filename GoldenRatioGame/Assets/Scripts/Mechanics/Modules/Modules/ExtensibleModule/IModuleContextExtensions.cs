using System.Collections.Generic;

namespace IM.Modules
{
    public interface IModuleContextExtensions
    {
        IReadOnlyList<IModuleExtension> Extensions { get; }
        
        T GetExtension<T>();
    }
}