using System.Linq;
using IM.Graphs;

namespace IM.Modules
{
    public class CommandGraphOperations : IGraphOperations
    {
        public IConditionalCommandModuleGraph Graph { get; set; }
        
        public CommandGraphOperations(IConditionalCommandModuleGraph graph)
        {
            Graph = graph;
        }

        public bool TryQuickAddModule(IModule module)
        {
            if (module is ICoreExtensibleModule)
            {
                if (!Graph.CanAddModule(module)) return false;
                
                Graph.AddModule(module);

                return true;
            }
            
            foreach (IPort port in module.Ports)
            {
                foreach (IPort targetPort in Graph.Modules.SelectMany(x => x.Ports))
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
            return TryQuickRemoveModule(Graph.Modules.LastOrDefault());
        }

        public bool TryQuickRemoveModule(IModule module)
        {
            if (!Graph.CanRemoveModule(module)) return false;
            
            Graph.RemoveModule(module);

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