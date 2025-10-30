using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public interface IModuleLayout
    {
        IEnumerable<IPortLayout> PortLayouts { get; }
        Sprite Sprite { get; }
        IPortLayout GetLayoutFor(IPort port);
    }
}