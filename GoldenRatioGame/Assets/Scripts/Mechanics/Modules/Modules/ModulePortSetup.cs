using System;
using System.Collections.Generic;
using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public class ModulePortSetup : MonoBehaviour
    {
        [SerializeField] private List<PortInfo> _portsInfos;
        private ExtensibleModuleMono _module;
        private Dictionary<IPort, PortInfo> _ports;
        
        public Dictionary<IPort, PortInfo> PortsInfos
        {
            get
            {
                if (_ports == null) InitializePorts();

                return _ports;
            }
        }

        private void InitializePorts()
        {
            _module = GetComponent<ExtensibleModuleMono>() ?? throw new NullReferenceException("GameModuleMono component not found");
                    
            _ports = new Dictionary<IPort, PortInfo>();
            
            foreach (PortInfo portInfo in _portsInfos)
            {
                IPort port = portInfo.Tag == null ? new Port(_module) : new TaggedPort(_module, portInfo.Tag);
                
                _module.PortsList.Add(port);
                _ports.Add(port,portInfo);
            }
        }
    }
}