namespace IM.Modules
{
    public interface IModuleEditingContextObserver
    {
        void OnContextCreated(IModuleEditingContext context);
    }
}