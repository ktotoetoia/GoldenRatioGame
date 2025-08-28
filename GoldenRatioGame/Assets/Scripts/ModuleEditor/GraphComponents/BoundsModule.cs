using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.ModuleEditor
{
    public class BoundsModule : Module,IHavePosition,IHaveSize,IContains
    {
        private Vector3 _position;
        public float DistanceBetweenPorts { get; set; } = 0.4f;

        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
                UpdatePortsPositions();
            }
        }

        public Vector3 Size { get; set; }
        
        public override void AddPort(IModulePort port)
        {
            base.AddPort(port);
            UpdatePortsPositions();
        }

        public bool Contains(Vector3 position)
        {
            return new Bounds(Position, Size).Contains(position);
        }

        private void UpdatePortsPositions()
        {
            float spacing = DistanceBetweenPorts;
            float totalWidth = -spacing;
            
            foreach (IHavePosition port in _ports.OfType<IHavePosition>())
            {
                float size = port is IHaveSize sized ? sized.Size.x : 0f;
                totalWidth += size + spacing;
            }

            float startX = Position.x - totalWidth / 2f;
            float currentX = startX;

            foreach (IHavePosition port in _ports.OfType<IHavePosition>())
            {
                float size = port is IHaveSize sized ? sized.Size.x : 0f;

                currentX += size / 2f;

                port.Position = new Vector3(currentX, Position.y, Position.z);

                currentX += size / 2f + spacing;
            }
            
            Size = new Vector3(totalWidth + spacing, Size.y,Size.z);
        }
    }
}