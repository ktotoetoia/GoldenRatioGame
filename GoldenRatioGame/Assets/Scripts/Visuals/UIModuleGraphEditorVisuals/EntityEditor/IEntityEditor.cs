using IM.Modules;

namespace IM.UI
{
    public interface IEntityEditor
    {
        void SetModuleEditingContext(IModuleEntity entity, IModuleEditingContext moduleEditingContext);
        void ClearModuleEditingContext();
    }
}