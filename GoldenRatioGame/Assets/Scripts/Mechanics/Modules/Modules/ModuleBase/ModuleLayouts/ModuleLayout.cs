using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleLayout : IModuleLayout
    {
        public IEnumerable<IPortLayout> PortLayouts { get; }
        public IGameModule Module { get; }
        public Sprite Sprite { get; }

        public ModuleLayout(IGameModule module, IEnumerable<IPortLayout> portLayouts, Sprite sprite)
        {
            Module = module;
            PortLayouts = portLayouts.ToList();
            Sprite = sprite;
        }
    }
}