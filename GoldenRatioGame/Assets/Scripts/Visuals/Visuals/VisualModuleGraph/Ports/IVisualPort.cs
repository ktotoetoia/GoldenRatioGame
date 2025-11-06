using IM.Graphs;
using UnityEngine;

namespace IM.ModuleGraph
{
    public interface IVisualPort : IPort, IHavePosition, IHaveRelativePosition
    {
        new IVisualModule Module { get; }
        new IVisualConnection Connection { get; }
        public Vector3 Normal { get; set; }
    }
}