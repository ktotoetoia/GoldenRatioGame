using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Modules
{
    public class GameObjectExtensionProvider : IExtensionProvider
    {
        private readonly List<IExtension> _extensions = new();

        public IReadOnlyList<IExtension> Extensions => _extensions;

        public GameObjectExtensionProvider(GameObject gameObject)
        {
            gameObject.GetComponents(_extensions);
        }
        
        public bool TryGetExtension<T>(out T extension)
        {
            extension = GetExtension<T>();
            return extension != null;
        }
        
        public T GetExtension<T>()
        {
            return Extensions.OfType<T>().FirstOrDefault();
        }

        public bool HasExtensionOfType<T>()
        {
            return Extensions.OfType<T>().Any();
        }

        public bool TryGetExtensions<T>(out IReadOnlyList<T> extensions)
        {
            extensions = GetExtensions<T>();
            return extensions != null && extensions.Count != 0;
        }

        public IReadOnlyList<T> GetExtensions<T>()
        {
            return _extensions.OfType<T>().ToList();
        }

        public int GetExtensionCount<T>()
        {
            return Extensions.OfType<T>().Count();
        }
    }
}