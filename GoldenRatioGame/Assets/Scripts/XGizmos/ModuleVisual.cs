using IM.Graphs;
using UnityEngine;

namespace IM.ModuleGraphGizmosDebug
{
    public class ModuleVisual : IModuleVisualWrapper
    {
        public Bounds Bounds { get; set; }
        public IModule Module { get; set; }

        public ModuleVisual(IModule module, Bounds bounds = new())
        {
            Module = module;
            Bounds = bounds;
        }
        
        public bool ContainsPoint(Vector3 point)
        {
            return Bounds.Contains(point);
        }

        public void MoveTo(Vector3 point)
        {
            Bounds = new Bounds(point, Bounds.size);
        }
    }
}