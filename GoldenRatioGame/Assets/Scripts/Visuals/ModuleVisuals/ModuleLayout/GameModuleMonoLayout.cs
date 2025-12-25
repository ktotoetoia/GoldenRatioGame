using System;
using System.Collections.Generic;
using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public class GameModuleMonoLayout : MonoBehaviour, IModuleLayout
    {
        [SerializeField] private GameObject _visualPrefab;
        [SerializeField] private List<PortInfo> _portsInfos;
        private readonly Dictionary<PortInfo, IPort>  _ports = new();
        private GameModuleMono _module;

        private void Awake()
        {
            _module = GetComponent<GameModuleMono>() ?? throw new NullReferenceException("GameModuleMono component not found");
            
            foreach (PortInfo portInfo in _portsInfos)
            {
                IPort port = portInfo.Tag == null ? new Port(_module) : new TaggedPort(_module, portInfo.Tag);
                
                _module.AddPort(port);
                _ports.Add(portInfo, port);
            }
        }

        public IVisualModule CreateVisualModule(IDictionary<IPort, IVisualPort> visualPortMap)
        {
            VisualModuleMono visualModule = Instantiate(_visualPrefab).GetComponent<VisualModuleMono>();

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