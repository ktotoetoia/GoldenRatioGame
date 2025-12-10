using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public interface IPortLayout
    {
        IPort Port { get; }
        Vector3 RelativePosition { get; }
        Vector3 Normal { get; }
    }
}