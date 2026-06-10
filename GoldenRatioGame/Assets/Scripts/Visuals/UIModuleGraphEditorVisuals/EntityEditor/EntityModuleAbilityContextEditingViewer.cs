using System.Collections.Generic;
using IM.Modules;
using IM.Visuals;
using UnityEngine;

namespace IM.UI
{
    [DefaultExecutionOrder(EntityContextEditorExecutionOrder)]
    public class EntityContextEditingViewer : MonoBehaviour, IEntityEditor
    {
        [SerializeField] private List<ContextViewer> _contextViewers;
        [SerializeField] private List<StorageView> _storageViews;
        [SerializeField] private List<StatPreviewContainer> _statPreviewContainers;
        private const int EntityContextEditorExecutionOrder = 10000;

        public void SetModuleEditingContext(IModuleEntity entity, IModuleEditingContext moduleEditingContext)
        {
            foreach (ContextViewer visualizer in _contextViewers) visualizer.SetContext(moduleEditingContext);
            foreach (StorageView storageView in _storageViews) storageView.SetStorage(moduleEditingContext.Storage);
            foreach (StatPreviewContainer statPreviewContainer in _statPreviewContainers) statPreviewContainer.StartPreview(entity, moduleEditingContext);
        }

        public void ClearModuleEditingContext()
        {
            foreach (ContextViewer visualizer in _contextViewers) visualizer.ClearContext();
            foreach (StorageView storageView in _storageViews) storageView.ClearStorage();
            foreach (StatPreviewContainer statPreviewContainer in _statPreviewContainers) statPreviewContainer.StopPreview();
        }
    }
}