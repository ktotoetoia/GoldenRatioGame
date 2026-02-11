using IM.Modules;

namespace IM.Visuals.Graph
{
    public interface IModulePreviewPlacer
    {
        bool IsPreviewing { get; }
        void StartPreview(IExtensibleModule module);
        void UpdatePreviewPosition();
        IExtensibleModule FinalizePreview();
        void StopPreview();
    }
}