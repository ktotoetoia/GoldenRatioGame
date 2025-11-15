using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public interface IModuleLayout
    {
        IGameModule Module { get; }
        IEnumerable<IPortLayout> PortLayouts { get; }
        Bounds Bounds { get; }
        Sprite Sprite { get; }
        IPortLayout GetPortLayoutFor(IPort port);
    }
}