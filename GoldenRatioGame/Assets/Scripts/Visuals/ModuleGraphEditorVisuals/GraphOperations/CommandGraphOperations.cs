using System.Linq;
using IM.Graphs;
using IM.Modules;

namespace IM.Visuals.Graph
{
    public class CommandGraphOperations : IGraphOperations
    {
        public IConditionalCommandModuleGraph Graph { get; set; }
        
        public CommandGraphOperations(IConditionalCommandModuleGraph graph)
        {
            Graph = graph;
        }


        public bool QuickAddModule(IModule module)
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

        public void QuickRemoveModule()
        {   
            IModule module = Graph.Modules.LastOrDefault();

            if (!Graph.CanRemoveModule(module)) return;
            
            Graph.RemoveModule(module);
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