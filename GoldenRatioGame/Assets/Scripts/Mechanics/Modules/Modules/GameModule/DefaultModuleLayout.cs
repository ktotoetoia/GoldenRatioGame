using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class DefaultModuleLayout : IModuleLayout
    {
        private readonly List<IPortSettings> _portSettings = new List<IPortSettings>();
        
        public IEnumerable<IPortSettings> PortSettings => _portSettings;

        public DefaultModuleLayout(IEnumerable<IPort> ports)
        {
            int i = 0;
            
            foreach (IPort port in ports)
            {
                _portSettings.Add(new PortSettings(port, Vector2.right * i ));
                i++;
            }
        }
    }
}