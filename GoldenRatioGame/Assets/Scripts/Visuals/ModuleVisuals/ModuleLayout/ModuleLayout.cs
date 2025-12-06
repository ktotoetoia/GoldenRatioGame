using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleLayout : MonoBehaviour, IModuleLayout, IPortInitializer
    {
        [SerializeField] private List<PortInfo> _portsInfos;
        private List<IPortLayout> _portLayouts;
        
        [field: SerializeField] public Sprite Icon { get; private set; }
        public IGameModule Module { get; private set; }
        public Bounds Bounds => Icon.bounds;

        public IEnumerable<IPortLayout> PortLayouts
        {
            get
            {
                if (_portLayouts == null)
                {
                    _portLayouts = new();
                    Module = GetComponent<IGameModule>() ?? throw new MissingComponentException(nameof(IGameModule));
            
                    foreach (PortInfo portInfo in _portsInfos)
                    {
                        IPort port;
                
                        if(portInfo.Tag !=null) port = new TaggedPort(Module, portInfo.Tag);
                        else port = new Port(Module);
                        _portLayouts.Add(new PortLayout(port,portInfo.Position,portInfo.Normal));
                    }
                }

                return _portLayouts;
            }
        }

        public IPortLayout GetPortLayoutFor(IPort port)
        {
            return PortLayouts.FirstOrDefault(x => x.Port == port);
        }

        public IEnumerable<IPort> GetPorts()
        {
            return PortLayouts.Select(x => x.Port);
        }
    }
}