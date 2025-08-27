using System.Collections.Generic;
using System.Linq;
using Unity.Android.Gradle;

namespace IM.Graphs
{
    public class Module : IModule
    {
        private List<IModulePort> _ports = new();
        
        public IEnumerable<IModulePort> Ports => _ports;
        public IEnumerable<IEdge> Edges => _ports.Select(x => x.Connection);

        public void AddPort(IModulePort port)
        {
            if (_ports.Contains(port))
            {
                throw new System.Exception("Port already exists");
            }
            
            _ports.Add(port);
        }
    }
}