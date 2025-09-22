using System;

namespace IM.Graphs
{
    //todo :: remove ModuleGraphEvents inheritance
    public class CoreModuleGraphEvents : ModuleGraphEvents, ICoreModuleGraph, ICoreModuleGraphEvents
    {
        private readonly ICoreModuleGraph _coreModuleGraph;

        public IModule CoreModule => _coreModuleGraph.CoreModule;
        public event Action<IModule> OnCoreModuleSet;

        public CoreModuleGraphEvents(IModule coreModule) : this(new CoreModuleGraph(coreModule))
        {

        }

        public CoreModuleGraphEvents(ICoreModuleGraph coreModuleGraph) : base(coreModuleGraph)
        {
            _coreModuleGraph = coreModuleGraph;
        }

        public void SetCoreModule(IModule module)
        {
            _coreModuleGraph.SetCoreModule(module);
            OnCoreModuleSet?.Invoke(module);
            CallOnModuleAdded(module);
        }
    }
}