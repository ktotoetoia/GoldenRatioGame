using System;
using System.Collections.Generic;
using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public class GameModuleMonoAnimationController : MonoBehaviour, IModuleAnimationController
    {
        [SerializeField] private GameObject _visualPrefab;
        [SerializeField] private List<PortInfo> _portsInfos;
        private readonly Dictionary<PortInfo, IPort>  _ports = new();
        private Dictionary<IPort,IVisualPort> _visualPorts;
        private GameModuleMono _module;
        
        public IAnimationModule ReferenceModule { get; private set; }

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

        public IVisualPort GetReferencePort(IPort port)
        {
            return _visualPorts[port];
        }

        public IAnimationModule CreateNewReferenceModule()
        {
            ReferenceModule?.Dispose();

            return ReferenceModule = CreateVisualModuleCopy(_visualPorts = new());
        }

        public IAnimationModule CreateVisualModuleCopy(IDictionary<IPort, IVisualPort> visualPortMap)
        {
            AnimationModule visualModule2 = Instantiate(_visualPrefab).GetComponent<AnimationModule>();

            foreach ((PortInfo portInfo, IPort port) in _ports)
            {
                IVisualPort visualPort = new VisualPort(visualModule2);
                
                visualModule2.HierarchyTransform.AddChild(visualPort.Transform);

                visualPort.Transform.LocalPosition = portInfo.Position;
                visualPort.Transform.LocalScale = Vector3.one;
                visualPort.Transform.LocalRotation = Quaternion.LookRotation(portInfo.Normal, Vector3.up);
                
                visualModule2.AddPort(visualPort);
                visualPortMap[port] = visualPort;
            }

            return visualModule2;
        }
    }
}