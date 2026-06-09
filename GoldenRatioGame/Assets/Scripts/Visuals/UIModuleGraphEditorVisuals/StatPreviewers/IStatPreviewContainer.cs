using IM.Modules;

namespace IM.Visuals
{
    public interface IStatPreviewContainer
    {
        void StartPreview(IModuleEntity entity, IModuleEditingContextReadOnly currentContext);
        void StopPreview();
    }
}