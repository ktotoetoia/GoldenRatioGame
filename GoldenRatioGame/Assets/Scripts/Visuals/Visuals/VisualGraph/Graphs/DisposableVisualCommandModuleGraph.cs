using System;
using System.Linq;

namespace IM.Visuals
{
    public class DisposableVisualCommandModuleGraph : VisualCommandModuleGraph, IDisposable
    {
        public DisposableVisualCommandModuleGraph()
        {
            
        }
        
        public DisposableVisualCommandModuleGraph(IHierarchyTransform transform) : base(transform)
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