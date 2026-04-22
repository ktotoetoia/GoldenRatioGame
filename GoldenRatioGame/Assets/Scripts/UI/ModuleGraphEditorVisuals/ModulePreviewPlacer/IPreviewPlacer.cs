using IM.Modules;
using IM.Visuals;

namespace IM.UI
{
    public interface IPreviewPlacer<TObject, out TVisual>
    {
        TVisual PreviewObject { get; }
        bool IsPreviewing { get; }
        void StartPreview(TObject module);
        void UpdatePreviewPosition();
        TObject FinalizePreview();
        void StopPreview();
    }
}