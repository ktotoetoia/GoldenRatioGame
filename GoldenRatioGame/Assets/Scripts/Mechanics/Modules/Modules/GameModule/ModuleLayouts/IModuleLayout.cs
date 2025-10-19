using System.Collections.Generic;
using UnityEngine;

namespace IM.Modules
{
    public interface IModuleLayout
    {
        IGameModule Module { get; }
        Bounds Bounds { get; }
        IEnumerable<IPortLayout> PortLayouts { get; }
    }
}