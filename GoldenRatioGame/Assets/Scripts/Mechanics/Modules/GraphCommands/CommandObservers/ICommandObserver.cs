namespace IM.Modules
{
    public interface ICommandObserver
    {
        void OnModuleAdded();
        void OnModuleRemoved();
    }
}