using IM.Modules;

namespace IM.Visuals.Graph
{
    public interface IEntityEditor
    {
        public void SetEntity(IModuleEntity entity);
        public void ClearEntity();
    }
}