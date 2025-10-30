using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public interface IPortLayout
    {
        Vector3 RelativePosition { get; }
        Vector3 Normal { get; }
        IPort Port { get; }
    }
}