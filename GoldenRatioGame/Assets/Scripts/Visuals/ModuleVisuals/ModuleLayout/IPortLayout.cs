using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public interface IPortLayout
    {
        Vector3 RelativePosition { get; }
        Vector3 Normal { get; }
        IPort Port { get; }
    }
}