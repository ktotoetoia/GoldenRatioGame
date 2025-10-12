using System.Collections.Generic;
using System.Linq;

namespace IM.Modules
{
    public class ModuleContextExtensions : IModuleContextExtensions
    {
        public IReadOnlyList<IModuleExtension> Extensions { get; }

        public ModuleContextExtensions(IEnumerable<IModuleExtension> extensions)
        {
            Extensions = extensions.ToList();
        }
        
        public T GetExtension<T>()
        {
            return Extensions.OfType<T>().FirstOrDefault();
        }
    }
}