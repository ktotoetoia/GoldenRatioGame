using System.Collections.Generic;
using System.Linq;

namespace IM.Graphs
{
    public class Module : IModule
    {
        private readonly List<IPort> _ports = new();
        
        public IEnumerable<IPort> Ports => _ports;
        public IEnumerable<IEdge> Edges => _ports.Where(x => x.IsConnected).Select(x => x.Connection);

        public virtual void AddPort(IPort port)
        {
            if (_ports.Contains(port))
            {
                throw new System.Exception("Port already exists");
            }
            
            _ports.Add(port);
        }
    }
}