using IM.Graphs;

namespace IM.Modules
{
    public class ModuleContextWrapper : Module, IHaveModuleContext
    {
        public IModuleContext ModuleContext { get; }
        
        public ModuleContextWrapper(IModuleContext moduleContext)
        {
            ModuleContext = moduleContext;
        }
    }
}