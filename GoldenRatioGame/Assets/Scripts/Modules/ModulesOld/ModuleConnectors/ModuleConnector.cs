namespace IM.Modules
{
    public class ModuleConnector : IModuleConnector
    {
        public IModule2 From { get; }
        public IModule2 To { get; set; }
        
        public ModuleConnector(IModule2 module)
        {
            From = module;
        }
    }
}