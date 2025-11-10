using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.ModuleGraph
{
    public interface IVisualModule : IModule
    {
        ITransform Transform { get; }
        Sprite Sprite { get; set; }
        
        new IEnumerable<IVisualPort> Ports { get; }
    }
}