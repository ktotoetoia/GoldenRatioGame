using IM.Modules;
using UnityEngine.UIElements;

namespace IM.Visuals
{
    public interface IStatPreviewer
    {
        VisualElement GetPreview(IModuleEntity entity, IModuleEditingContextReadOnly currentContext);
        void UpdatePreview(VisualElement previewElement, IModuleEntity entity, IModuleEditingContextReadOnly currentContext);
    }
}