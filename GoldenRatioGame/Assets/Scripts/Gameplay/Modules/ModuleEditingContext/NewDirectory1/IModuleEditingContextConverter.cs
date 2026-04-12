namespace IM.Modules
{
    public interface IModuleEditingContextConverter
    {
        IModuleEditingContextReadOnly ToReadOnly(IModuleEditingContext moduleEditingContext);
        IModuleEditingContext ToMutable(IModuleEditingContextReadOnly moduleEditingContextReadOnly);
    }
}