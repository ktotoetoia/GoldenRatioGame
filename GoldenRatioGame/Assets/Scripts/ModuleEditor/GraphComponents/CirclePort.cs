using IM.Graphs;
using UnityEngine;

namespace IM.ModuleEditor
{
    public class CirclePort : ModulePort, IHavePosition,IHaveSize,IContains
    {
        public Vector3 Position { get; set; }
        public Vector3 Size { get; set; } = Vector3.one * 0.2f; 
        
        public CirclePort(IModule module, PortDirection direction) : base(module, direction)
        {
            
        }

        public bool Contains(Vector3 position)
        {
            return Vector3.Distance(Position, position) <= Size.x;
        }
    }
}