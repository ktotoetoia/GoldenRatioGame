using IM.Modules;

namespace IM.Visuals.Graph
{
    public interface IModuleGraphEditorView
    {
        public void SetModuleEntity(IModuleEntity moduleEntity);
        public void ClearModuleEntity();
    }
}