using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    [RequireComponent(typeof(GameModuleMono))]
    public class PortInitializer : MonoBehaviour, IPortInitializer
    {
        [SerializeField] private List<PortInfo> _portsInfos;

        public IEnumerable<(IPort, IPortLayout)> GetPorts(IGameModule module)
        {
            List<(IPort,IPortLayout)> ports = new();

            foreach (PortInfo portInfo in _portsInfos)
            {
                IPort port;
                
                if(portInfo.Tag !=null) port = new TaggedPort(module, portInfo.Tag);
                else port = new Port(module);
                
                ports.Add((port,new PortLayout(port,portInfo.Position,portInfo.Normal)));
            }

            return ports;
        }
    }
}