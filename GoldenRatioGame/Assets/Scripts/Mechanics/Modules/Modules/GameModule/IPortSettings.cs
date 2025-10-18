using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public interface IPortSettings
    {
        IPort Port { get; }
        Vector3 RelativePosition { get; }
    }
}