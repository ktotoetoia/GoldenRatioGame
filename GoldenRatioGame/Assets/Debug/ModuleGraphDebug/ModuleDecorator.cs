using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.ModuleEditor
{
    public class ModuleDecorator : IModule,IHavePosition,IHaveSize,IContains
    {
        private Vector3 _position;
        
        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
            }
        }
        public IModule Module { get; }
        public float DistanceBetweenPorts { get; set; } = 0.4f;
        public Vector3 Size { get; set; }

        public IEnumerable<IEdge> Edges => Module.Edges;
        public IEnumerable<IModulePort> Ports => Module.Ports;
        
        public ModuleDecorator(IModule module)
        {
            Module = module;
        }

        public bool Contains(Vector3 position)
        {
            return new Bounds(Position, Size).Contains(position);
        }
    }
}