using System.Collections.Generic;

namespace IM.Modules
{
    public interface IExtensionProvider
    {
        IReadOnlyList<IExtension> Extensions { get; }

        T GetExtension<T>();
        bool TryGetExtension<T>(out T extension);

        IReadOnlyList<T> GetExtensions<T>();
        bool TryGetExtensions<T>(out IReadOnlyList<T> extensions);

        bool HasExtensionOfType<T>();
        int GetExtensionCount<T>();
    }
}