using System.Collections.Generic;
using System.Linq;
using IM.Base;
using IM.Graphs;
using UnityEngine;

namespace IM.ModuleGraphGizmosDebug
{
    public class ModuleWrapper : IModuleVisualWrapper
    {
        private readonly EnumerableWrapper<IModulePort, IPortVisualWrapper> _ports;
        private const float _portRadius = 0.2f;
        
        public IModule Module { get; }
        public IVisual Visual { get; }

        public IEnumerable<IPortVisualWrapper> Ports
        {
            get
            {
                ArrangePorts();
                return _ports;
            }
        }

        public ModuleWrapper(IModule module, IVisual visual)
        {
            Module = module;
            Visual = visual;
            _ports= new EnumerableWrapper<IModulePort, IPortVisualWrapper>(Module.Ports,new PortWrapperFactory(Visual.Position, _portRadius));
        }

        private void ArrangePorts()
        { 
            List<IPortVisualWrapper > ports = new List<IPortVisualWrapper>(_ports);
            
            for (int i = 0; i < ports.Count; i++)
            {
                Vector3 position = Visual.Position + Vector3.right * i;
                ports[i].Visual.MoveTo(position);
            }
        }
    }
}