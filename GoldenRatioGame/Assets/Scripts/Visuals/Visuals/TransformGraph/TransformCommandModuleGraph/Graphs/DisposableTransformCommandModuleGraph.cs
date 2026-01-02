using System;
using System.Linq;

namespace IM.Visuals
{
    public class DisposableTransformCommandModuleGraph : TransformCommandModuleGraph, IDisposable
    {
        public DisposableTransformCommandModuleGraph(IHierarchyTransform transform) : base(transform)
        {
            
        }
        
        public void Dispose()
        {
            foreach (IDisposable module in Modules.OfType<IDisposable>())
            {
                module.Dispose();
            }
        }
    }
}