using IM.Graphs;
using UnityEngine;

namespace IM.ModuleGraphGizmosDebug
{
    public interface IModuleVisualWrapper
    {
        IModule Module { get; }
        
        bool ContainsPoint(Vector3 point);
        void MoveTo(Vector3 point);
    }
}