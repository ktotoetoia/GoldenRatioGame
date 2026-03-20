using IM.Modules;
using IM.Visuals;

namespace IM.UI
{
    public interface IModulePreviewPlacer
    {
        IModuleVisualObject PreviewObject { get; }
        bool IsPreviewing { get; }
        void StartPreview(IExtensibleModule module);
        void UpdatePreviewPosition();
        IExtensibleModule FinalizePreview();
        void StopPreview();
    }
}