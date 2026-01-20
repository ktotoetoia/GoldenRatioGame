using System;
using System.Collections.Generic;
using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    [DefaultExecutionOrder(-10)]
    public class ModulePortSetup : MonoBehaviour
    {
        [SerializeField] private List<PortInfo> _portsInfos;
        private ExtensibleModuleMono _module;
        
        public Dictionary<IPort, PortInfo> PortsInfos { get; } = new();
        
        private void Awake()
        {
            _module = GetComponent<ExtensibleModuleMono>() ?? throw new NullReferenceException("GameModuleMono component not found");
            
            foreach (PortInfo portInfo in _portsInfos)
            {
                IPort port = portInfo.Tag == null ? new Port(_module) : new TaggedPort(_module, portInfo.Tag);
                
                _module.AddPort(port);
                PortsInfos.Add(port,portInfo);
            }
        }
    }
}