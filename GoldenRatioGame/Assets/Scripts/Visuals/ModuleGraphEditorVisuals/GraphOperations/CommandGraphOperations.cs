using System.Linq;
using IM.Graphs;
using IM.Modules;

namespace IM.Visuals.Graph
{
    public class CommandGraphOperations : IGraphOperations
    {
        private readonly IConditionalCommandModuleGraph _graph;
        
        public CommandGraphOperations(IConditionalCommandModuleGraph graph)
        {
            _graph = graph;
        }
        
        public bool QuickAddModule(IModule module)
        {
            if (module is ICoreExtensibleModule)
            {
                if (!_graph.CanAddModule(module)) return false;
                
                _graph.AddModule(module);

                return true;
            }
            
            foreach (IPort port in module.Ports)
            {
                foreach (IPort targetPort in _graph.Modules.SelectMany(x => x.Ports))
                {
                    if (!_graph.CanAddAndConnect(module, port, targetPort)) continue;
                        
                    _graph.AddAndConnect(module, port, targetPort);
                            
                    return true;
                }
            }

            return false;
        }

        public void QuickRemoveModule()
        {   
            IModule module = _graph.Modules.LastOrDefault();

            if (!_graph.CanRemoveModule(module)) return;
            
            _graph.RemoveModule(module);
        }

        public void Undo(int count)
        {
            if(_graph.CanUndo(count)) _graph.Undo(count);
        }
        
        public void Redo(int count)
        {
            if(_graph.CanRedo(count)) _graph.Redo(count);
        }
    }
}