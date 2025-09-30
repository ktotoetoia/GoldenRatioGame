using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.ModuleGraphGizmosDebug
{
    public interface IModuleVisualWrapper
    {
        IModule Module { get; }
        IVisual Visual { get; }

        IEnumerable<IPortVisualWrapper> Ports { get; }
    }
}