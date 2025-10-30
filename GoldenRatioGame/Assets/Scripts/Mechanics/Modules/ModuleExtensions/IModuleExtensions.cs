using System.Collections.Generic;

namespace IM.Modules
{
    public interface IModuleExtensions
    {
        IReadOnlyList<IModuleExtension> Extensions { get; }
        
        void AddExtension(IModuleExtension extension);
        void RemoveExtension(IModuleExtension extension);
        
        T GetExtension<T>();
    }
}