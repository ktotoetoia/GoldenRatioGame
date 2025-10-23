using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleExtensionsMono : MonoBehaviour, IModuleExtensions
    {
        private readonly List<IModuleExtension> _extensions = new();

        public IReadOnlyList<IModuleExtension> Extensions => _extensions;
        
        private void Awake()
        {
            GetComponents(_extensions);
        }
        
        public T GetExtension<T>()
        {
            return Extensions.OfType<T>().FirstOrDefault();
        }
    }
}