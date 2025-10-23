using System.Collections.Generic;
using UnityEngine;

namespace IM.Modules
{
    public interface IModuleLayout
    {
        IGameModule Module { get; }
        IEnumerable<IPortLayout> PortLayouts { get; }
        Sprite Sprite { get; }
    }
}