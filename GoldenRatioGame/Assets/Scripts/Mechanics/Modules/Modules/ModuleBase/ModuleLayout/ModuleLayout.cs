using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleLayout : IModuleLayout
    {
        public IGameModule Module { get; }
        public IEnumerable<IPortLayout> PortLayouts { get; }
        public Sprite Sprite { get; }
        
        public ModuleLayout(IGameModule module,IEnumerable<IPortLayout> portLayouts, Sprite sprite)
        {
            Module = module;
            PortLayouts = portLayouts.ToList();
            Sprite = sprite;
        }

        public IPortLayout GetPortLayoutFor(IPort port)
        {
            return PortLayouts.FirstOrDefault(x => x.Port == port);
        }
    }
}