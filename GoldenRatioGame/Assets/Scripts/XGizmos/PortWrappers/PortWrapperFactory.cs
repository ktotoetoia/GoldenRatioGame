using IM.Base;
using IM.Graphs;
using UnityEngine;

namespace IM.ModuleGraphGizmosDebug
{
    public class PortWrapperFactory : IFactory<PortWrapper, IModulePort>
    {
        public Vector3 Position { get; set; }
        public float Size { get; set; }

        public PortWrapperFactory(Vector3 position, float size)
        {
            Position = position;
            Size = size;
        }
        
        public PortWrapper Create(IModulePort port)
        {
            return new PortWrapper(port,new  CircleVisual(Position,Size));
        }
    }
}