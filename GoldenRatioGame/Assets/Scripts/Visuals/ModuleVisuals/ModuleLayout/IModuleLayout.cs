using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public interface IModuleLayout : IModuleExtension
    {
        IGameModule Module { get; }
        IEnumerable<IPortLayout> PortLayouts { get; }
        Bounds Bounds { get; }
        Sprite Icon { get; }
        IPortLayout GetPortLayoutFor(IPort port);
    }
}