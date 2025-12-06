using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public class PortLayout : IPortLayout
    {
        public Vector3 RelativePosition { get; }
        public Vector3 Normal { get; }
        public IPort Port { get; }
        
        public PortLayout(IPort port,Vector3 position, Vector3 normal)
        {
            Port = port;
            RelativePosition = position;
            Normal = normal;
        }
    }
}