using System.Collections.Generic;

namespace IM.Modules
{
    public interface IExtensionController
    {
        IReadOnlyList<IExtension> Extensions { get; }
        
        void AddExtension(IExtension extension);
        void RemoveExtension(IExtension extension);
        
        T GetExtension<T>();
        bool TryGetExtension<T>(out T extension);
        bool HasExtensionOfType<T>();
        List<T> GetExtensions<T>();
        bool TryGetExtension<T>(out List<T> extensions);
    }
}