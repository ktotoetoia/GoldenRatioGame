using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleLayout : MonoBehaviour, IModuleLayout, IPortInitializer
    {
        [SerializeField] private GameObject _visualPrefab;
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
        
        public IVisualModule CreateVisualModule(IDictionary<IPort, IVisualPort> visualPortMap)
        {            
            VisualModuleMono visualModule = Instantiate(_visualPrefab).GetComponent<VisualModuleMono>();
            
            visualModule.Icon = Icon;
            visualModule.HierarchyTransform.LocalScale = Bounds.size;

            foreach (IPortLayout portLayout in PortLayouts)
            {
                IVisualPort visualPort = new VisualPort(visualModule);
                
                visualModule.HierarchyTransform.AddChild(visualPort.Transform);

                visualPort.Transform.LocalPosition = portLayout.RelativePosition;
                visualPort.Transform.LocalScale = Vector3.one;
                visualPort.Transform.LocalRotation = Quaternion.LookRotation(portLayout.Normal, Vector3.up);
                
                visualModule.AddPort(visualPort);
                visualPortMap[portLayout.Port] = visualPort;
            }

            return visualModule;
        }

        public IEnumerable<IPort> GetPorts()
        {
            return PortLayouts.Select(x => x.Port);
        }
    }
}