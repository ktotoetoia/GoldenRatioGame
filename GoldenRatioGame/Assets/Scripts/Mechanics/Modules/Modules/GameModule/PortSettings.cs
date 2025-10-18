using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class PortSettings : IPortSettings
    {
        public IPort Port { get; }
        public Vector3 RelativePosition { get; }
        
        public PortSettings(IPort port, Vector3 relativePosition)
        {
            RelativePosition = relativePosition;
            Port = port;
        }
    }
}