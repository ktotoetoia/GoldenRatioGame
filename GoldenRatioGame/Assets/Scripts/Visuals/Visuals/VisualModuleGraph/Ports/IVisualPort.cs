using IM.Graphs;
using UnityEngine;

namespace IM.ModuleGraph
{
    public interface IVisualPort : IPort
    {
        ITransform Transform { get; }
        new IVisualModule Module { get; }
        new IVisualConnection Connection { get; }
    }
}