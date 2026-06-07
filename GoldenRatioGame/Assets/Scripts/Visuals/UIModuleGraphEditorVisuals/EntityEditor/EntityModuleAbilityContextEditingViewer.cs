using System.Collections.Generic;
using IM.Modules;
using UnityEngine;

namespace IM.UI
{
    [DefaultExecutionOrder(EntityContextEditorExecutionOrder)]
    public class EntityModuleAbilityContextEditingViewer : MonoBehaviour, IEntityEditor
    {
        [SerializeField] private List<ContextViewer> _contextViewers;
        [SerializeField] private List<StorageView> _storageViews;

        private const int EntityContextEditorExecutionOrder = 10000;

        public void SetModuleEditingContext(IModuleEditingContext moduleEditingContext)
        {
            foreach (ContextViewer visualizer in _contextViewers) visualizer.SetContext(moduleEditingContext);
            foreach (StorageView storageView in _storageViews) storageView.SetStorage(moduleEditingContext.Storage);
        }

        public void ClearModuleEditingContext()
        {
            foreach (ContextViewer visualizer in _contextViewers) visualizer.ClearContext();
            foreach (StorageView storageView in _storageViews) storageView.ClearStorage();
        }
    }
}