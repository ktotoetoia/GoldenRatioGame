namespace IM.UI
{
    public interface IPreviewPlacer<TObject, out TVisual>
    {
        TVisual PreviewVisual { get; }
        TObject PreviewObject { get; }
        bool IsPreviewing { get; }
        void StartPreview(TObject module);
        void UpdatePreviewPosition();
        TObject FinalizePreview();
        void StopPreview();
    }
}