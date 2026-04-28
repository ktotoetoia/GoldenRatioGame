using System.Linq;
using IM.Graphs;

namespace IM.Modules
{
    public static class GraphEditingServiceExtensions
    {
        public static bool TryQuickAddModule<T>(this IGraphEditingService<T> graphEditingService, IDataModule<T> module)
        {
            if (module.Value is ICoreExtensibleItem)
            {
                if (!graphEditingService.CanAdd(module)) return false;
                
                graphEditingService.Add(module);

                return true;
            }
            
            foreach (IDataPort<T> port in module.DataPorts)
            {
                foreach (IDataPort<T> targetPort in graphEditingService.GraphReadOnly.DataModules.SelectMany(x => x.DataPorts))
                {
                    if (!graphEditingService.CanAddAndConnect(module, port, targetPort)) continue;
                    
                    graphEditingService.AddAndConnect(module, port, targetPort);
                    
                    return true;
                }
            }

            return false;
        }

        public static bool TryQuickRemoveModule<T>(this IGraphEditingService<T> graphEditingService)
        {
            return TryQuickRemoveModule(graphEditingService, graphEditingService.GraphReadOnly.DataModules.LastOrDefault());
        }

        public static bool TryQuickRemoveModule<T>(this IGraphEditingService<T> graphEditingService, IDataModule<T> module)
        {
            if (!graphEditingService.CanRemove(module)) return false;
            
            graphEditingService.Remove(module);

            return true;
        }
    }
}