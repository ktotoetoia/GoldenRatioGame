using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IM.Graphs;
using IM.Modules;

namespace IM.ModuleGraph
{
    public class VisualModule : IVisualModule
    {
        private readonly List<IVisualPort> _ports = new();
        
        public IEnumerable<IEdge> Edges => _ports.Where(x => x.IsConnected).Select(x => x.Connection);
        public IEnumerable<IPort> Ports => _ports;
        public IEnumerable<IVisualPort> VisualPorts => _ports;

        public Vector3 Position { get; set; }
        
        public VisualModule(Vector3 position = new())
        {
            Position = position;
        }
        
        public void AddPort(IVisualPort port)
        {
            _ports.Add(port);
        }
    }

    public class VisualModuleGraph
    {
        private readonly IVisualModule _core;
        private readonly Dictionary<IGameModule, IVisualModule> _modules = new();
        
        public VisualModuleGraph(IGameModule core)
        {
            foreach ((IGameModule module, IPort from)in new BreadthFirstTraversal().EnumerateModules<IGameModule,IPort>(core))
            {
                VisualModule visualModule = new VisualModule();

                foreach (IPortLayout portLayout in module.ModuleLayout.PortLayouts)
                {
                    visualModule.AddPort(new VisualPort(visualModule, portLayout.RelativePosition,portLayout.Normal));
                }

                if (module == core)
                {
                    _core = visualModule;
                }
                
                _modules.Add(module, visualModule);
            }

            foreach ((IGameModule module, IPort from)in new BreadthFirstTraversal().EnumerateModules<IGameModule,IPort>(core))
            {
                
            }
        }
    }
}