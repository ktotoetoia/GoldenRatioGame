using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleLayout : IModuleLayout
    {
        public IEnumerable<IPortLayout> PortLayouts { get; }
        public Sprite Sprite { get; }

        public ModuleLayout(IEnumerable<IPortLayout> portLayouts, Sprite sprite)
        {
            PortLayouts = portLayouts.ToList();
            Sprite = sprite;
        }

        public IPortLayout GetLayoutFor(IPort port)
        {
            return PortLayouts.FirstOrDefault(x => x.Port == port);
        }
    }
}