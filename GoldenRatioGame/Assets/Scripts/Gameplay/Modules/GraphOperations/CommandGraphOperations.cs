using System.Linq;
using IM.Graphs;
using IM.Storages;

namespace IM.Modules
{
    public class CommandGraphOperations<T> : IGraphOperations<T> where T : class,IStorableReadOnly
    {
        public IDataModuleGraphReadOnly<T> Graph { get; }
        public GraphEditingService<T> GraphEditingService { get; }
        
        public CommandGraphOperations(IDataModuleGraphReadOnly<T> graph, GraphEditingService<T> graphEditingService)
        {
            GraphEditingService = graphEditingService;
            Graph = graph;
        }

        public bool TryQuickAddModule(IDataModule<T> module)
        {
            if (module.Value is ICoreExtensibleItem)
            {
                if (!GraphEditingService.CanAdd(module)) return false;
                
                GraphEditingService.Add(module);

                return true;
            }
            
            foreach (IDataPort<T> port in module.DataPorts)
            {
                foreach (IDataPort<T> targetPort in Graph.DataModules.SelectMany(x => x.DataPorts))
                {
                    if (!GraphEditingService.CanAddAndConnect(module, port, targetPort)) continue;
                    
                    GraphEditingService.AddAndConnect(module, port, targetPort);
                    
                    return true;
                }
            }

            return false;
        }

        public bool TryQuickRemoveModule()
        {
            return TryQuickRemoveModule(Graph.DataModules.LastOrDefault());
        }

        public bool TryQuickRemoveModule(IDataModule<T> module)
        {
            if (!GraphEditingService.CanRemove(module)) return false;
            
            GraphEditingService.Remove(module);

            return true;
        }
    }
}