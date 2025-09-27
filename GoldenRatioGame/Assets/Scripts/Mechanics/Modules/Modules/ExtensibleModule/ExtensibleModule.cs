using System.Collections.Generic;
using System.Linq;
using IM.Graphs;

namespace IM.Modules
{
    public class ExtensibleModule : IExtensibleModule
    {
        private readonly List<IModulePort> _ports;
        private readonly List<IModuleExtension> _extensions;
        
        public IReadOnlyList<IModuleExtension> Extensions => _extensions;
        public IEnumerable<IEdge> Edges => _ports.Select(x => x.Connection);
        public IEnumerable<IModulePort> Ports => _ports;

        public ExtensibleModule() : this(new  List<IModulePort>(), new  List<IModuleExtension>())
        {
            
        }

        public ExtensibleModule(List<IModulePort> ports, List<IModuleExtension> extensions)
        {
            _ports =  ports;
            _extensions = extensions;
        }

        public void AddPort(IModulePort port)
        {
            _ports.Add(port);
        }

        public void RemovePort(IModulePort port)
        {
            _ports.Remove(port);
        }

        public void AddModuleExtension(IModuleExtension extension)
        {
            _extensions.Add(extension);
        }

        public void RemoveModuleExtension(IModuleExtension extension)
        {
            _extensions.Remove(extension);
        }

        public T GetExtension<T>()
        {
            return _extensions.OfType<T>().FirstOrDefault();
        }
    }
}