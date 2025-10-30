using System.Collections.Generic;
using System.Linq;
using IM.Base;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class SquareModuleLayoutFactory : IFactory<ModuleLayout, IGameModule, Sprite>
    {
        public ModuleLayout Create(IGameModule module, Sprite sprite)
        {
            List<IPortLayout> portLayouts = new();
            
            List<IPort> ports = module.Ports.ToList();
            int n = ports.Count;
            if (n == 1)
            {
                Vector3 pos = Vector3.zero;
                portLayouts.Add(new PortLayout(ports[0], pos, Vector3.up));
            }

            int side = 2;
            while (4 * (side - 1) < n) side++;
            float half = (side - 1) / 2f;
            List<Vector3> positions = new List<Vector3>();

            for (int i = 0; i < side; i++) positions.Add(new Vector3(-half + i, half));
            for (int i = 1; i < side; i++) positions.Add(new Vector3(half, half - i));
            for (int i = 1; i < side; i++) positions.Add(new Vector3(half - i, -half));
            for (int i = 1; i < side - 1; i++) positions.Add(new Vector3(-half, -half + i));
            
            for (int i = 0; i < n; i++)
                portLayouts.Add(new PortLayout(ports[i], positions[i],positions[i]));
            
            return new ModuleLayout(portLayouts,sprite);
        }
    }
}