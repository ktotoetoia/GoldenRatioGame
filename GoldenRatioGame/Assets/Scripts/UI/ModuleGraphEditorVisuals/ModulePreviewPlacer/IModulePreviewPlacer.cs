using IM.Modules;
using IM.Visuals;

namespace IM.UI
{
    public interface IModulePreviewPlacer
    {
        IModuleVisualObject PreviewObject { get; }
        bool IsPreviewing { get; }
        void StartPreview(IExtensibleItem module);
        void UpdatePreviewPosition();
        IExtensibleItem FinalizePreview();
        void StopPreview();
    }
}