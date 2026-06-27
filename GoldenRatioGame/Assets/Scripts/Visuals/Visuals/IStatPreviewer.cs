using UnityEngine;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public interface IStatPreviewer
    {
        VisualElement GetPreview(object item);
        void UpdatePreview(VisualElement previewElement, object item);
    }
}