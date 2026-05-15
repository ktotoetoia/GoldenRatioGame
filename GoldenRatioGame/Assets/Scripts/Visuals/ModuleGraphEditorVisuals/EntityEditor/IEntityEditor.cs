using IM.Modules;

namespace IM.UI
{
    public interface IEntityEditor
    {
        void SetModuleEditingContext(IModuleEditingContext moduleEditingContext);
        void ClearModuleEditingContext();
    }
}