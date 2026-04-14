using System.Linq;
using IM.Graphs;

namespace IM.Modules
{
    public class CommandGraphOperations<T> : IGraphOperations<T>
    {
        public IConditionalCommandDataModuleGraph<T> Graph { get; set; }
        
        public CommandGraphOperations(IConditionalCommandDataModuleGraph<T> graph)
        {
            Graph = graph;
        }

        public bool TryQuickAddModule(IDataModule<T> module)
        {
            if (module.Value is ICoreExtensibleItem)
            {
                if (!Graph.CanAdd(module)) return false;
                
                Graph.Add(module);

                return true;
            }
            
            foreach (IDataPort<T> port in module.DataPorts)
            {
                foreach (IDataPort<T> targetPort in Graph.DataModules.SelectMany(x => x.DataPorts))
                {
                    if (!Graph.CanAddAndConnect(module, port, targetPort)) continue;
                    
                    Graph.AddAndConnect(module, port, targetPort);
                    
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
            if (!Graph.CanRemove(module)) return false;
            
            Graph.Remove(module);

            return true;
        }

        public void Undo(int count)
        {
            if(Graph.CanUndo(count)) Graph.Undo(count);
        }
        
        public void Redo(int count)
        {
            if(Graph.CanRedo(count)) Graph.Redo(count);
        }
    }
}