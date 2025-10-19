using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class PortLayout : IPortLayout
    {
        public Vector3 RelativePosition { get; }
        public IPort Port { get; }
        
        public PortLayout(IPort port,Vector3 position)
        {
            Port = port;
            RelativePosition = position;
        }
    }
}