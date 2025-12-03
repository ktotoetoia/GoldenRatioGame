using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public interface IVisualModule : IModule
    {
        ITransform Transform { get; }
        Sprite Icon { get;}
        
        new IEnumerable<IVisualPort> Ports { get; }
    }
}