using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public class GameModuleMonoLayout : MonoBehaviour, IModuleLayout
    {
        [SerializeField] private GameObject _visualPrefab;
        [SerializeField] private List<PortInfo> _portsInfos;
        [SerializeField] private Sprite _sprite;
        private readonly Dictionary<PortInfo, IPort>  _ports = new();
        private GameModuleMono _module;

        private void Awake()
        {
            _module = GetComponent<GameModuleMono>() ?? throw new NullReferenceException("GameModuleMono component not found");
            
            foreach (PortInfo portInfo in _portsInfos)
            {
                IPort port;
                
                if(portInfo.Tag !=null) port = new TaggedPort(_module, portInfo.Tag);
                else port = new Port(_module);
                
                _module.AddPort(port);
                _ports.Add(portInfo, port);
            }
        }

        public IVisualModule CreateTemporaryVisualModule(IDictionary<IPort, IVisualPort> visualPortMap)
        {
            VisualModuleMono visualModule = Instantiate(_visualPrefab).GetComponent<VisualModuleMono>();
            
            visualModule.Icon = _sprite;
            visualModule.HierarchyTransform.LocalScale = _sprite.bounds.size;

            foreach ((PortInfo portInfo, IPort port) in _ports)
            {
                IVisualPort visualPort = new VisualPort(visualModule);
                
                visualModule.HierarchyTransform.AddChild(visualPort.Transform);

                visualPort.Transform.LocalPosition = portInfo.Position;
                visualPort.Transform.LocalScale = Vector3.one;
                visualPort.Transform.LocalRotation = Quaternion.LookRotation(portInfo.Normal, Vector3.up);
                
                visualModule.AddPort(visualPort);
                visualPortMap[port] = visualPort;
            }

            return visualModule;
        }
    }
}