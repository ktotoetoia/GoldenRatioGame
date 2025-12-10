using System.Collections.Generic;
using IM.Graphs;
using IM.Modules;
using UnityEngine;

namespace IM.Visuals
{
    public interface IModuleLayout : IModuleExtension
    {
        IGameModule Module { get; }
        IEnumerable<IPortLayout> PortLayouts { get; }
        Bounds Bounds { get; }
        RuntimeAnimatorController AnimatorController { get; }
        Sprite Icon { get; }
    }
}