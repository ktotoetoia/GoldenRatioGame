namespace IM.Modules
{
    public interface IModuleConnector
    {
        public IModule2 From { get; }
        public IModule2 To { get; }
    }
}