namespace IM.Modules
{
    public class ModuleComposition : IModuleComposition
    {
        public IModule2 CentralModule { get; }
        
        public ModuleComposition(IModule2 centralModule)
        {
            CentralModule = centralModule;
        }
    }
}