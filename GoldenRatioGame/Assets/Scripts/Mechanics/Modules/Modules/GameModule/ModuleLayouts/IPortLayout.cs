using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public interface IPortLayout
    {
        Vector3 RelativePosition { get; }
        IPort Port { get; }
    }
}