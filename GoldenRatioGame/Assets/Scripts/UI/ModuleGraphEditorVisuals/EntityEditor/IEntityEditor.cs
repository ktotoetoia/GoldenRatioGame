using IM.Modules;

namespace IM.UI
{
    public interface IEntityEditor
    {
        void SetEntity(IModuleEntity entity);
        void ForceClearEntity();
    }
}